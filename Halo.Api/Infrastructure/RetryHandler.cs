using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace Halo.Api.Infrastructure;

/// <summary>
/// HTTP message handler that implements retry logic with exponential backoff
/// </summary>
internal sealed class RetryHandler(
	int maxRetryAttempts,
	TimeSpan retryDelay,
	bool useExponentialBackoff,
	TimeSpan maxRetryDelay,
	ILogger? logger) : DelegatingHandler
{
	private readonly int _maxRetryAttempts = maxRetryAttempts;
	private readonly TimeSpan _retryDelay = retryDelay;
	private readonly bool _useExponentialBackoff = useExponentialBackoff;
	private readonly TimeSpan _maxRetryDelay = maxRetryDelay;
	private readonly ILogger? _logger = logger;

	private static readonly HttpStatusCode[] RetryableStatusCodes =
	[
		HttpStatusCode.RequestTimeout,
		HttpStatusCode.TooManyRequests,
		HttpStatusCode.InternalServerError,
		HttpStatusCode.BadGateway,
		HttpStatusCode.ServiceUnavailable,
		HttpStatusCode.GatewayTimeout
	];

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		var attempt = 0;
		Exception? lastException = null;

		while (attempt <= _maxRetryAttempts)
		{
			try
			{
				var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

				// Return successful responses or non-retryable errors
				if (response.IsSuccessStatusCode || !IsRetryableStatusCode(response.StatusCode))
				{
					if (attempt > 0)
					{
						_logger?.LogInformation(
							"HTTP request succeeded on attempt {Attempt} for {Method} {Uri}",
							attempt + 1,
							request.Method,
							request.RequestUri);
					}

					return response;
				}

				// Dispose failed response to free resources
				response.Dispose();

				if (attempt == _maxRetryAttempts)
				{
					_logger?.LogWarning(
						"HTTP request failed after {MaxAttempts} attempts for {Method} {Uri} with status {StatusCode}",
						_maxRetryAttempts + 1,
						request.Method,
						request.RequestUri,
						response.StatusCode);

					// Create a new response for the final failure
					return new HttpResponseMessage(response.StatusCode)
					{
						RequestMessage = request,
						ReasonPhrase = $"Failed after {_maxRetryAttempts + 1} attempts"
					};
				}

				_logger?.LogWarning(
					"HTTP request failed on attempt {Attempt}, retrying in {Delay}ms for {Method} {Uri} (Status: {StatusCode})",
					attempt + 1,
					CalculateDelay(attempt).TotalMilliseconds,
					request.Method,
					request.RequestUri,
					response.StatusCode);
			}
			catch (Exception ex) when (IsRetryableException(ex))
			{
				lastException = ex;

				if (attempt == _maxRetryAttempts)
				{
					_logger?.LogError(ex,
						"HTTP request failed after {MaxAttempts} attempts for {Method} {Uri}",
						_maxRetryAttempts + 1,
						request.Method,
						request.RequestUri);
					throw;
				}

				_logger?.LogWarning(ex,
					"HTTP request failed on attempt {Attempt}, retrying in {Delay}ms for {Method} {Uri}",
					attempt + 1,
					CalculateDelay(attempt).TotalMilliseconds,
					request.Method,
					request.RequestUri);
			}

			// Wait before retrying
			if (attempt < _maxRetryAttempts)
			{
				await Task.Delay(CalculateDelay(attempt), cancellationToken).ConfigureAwait(false);
			}

			attempt++;
		}

		// This should never be reached due to the logic above, but just in case
		throw lastException ?? new HttpRequestException($"Request failed after {_maxRetryAttempts + 1} attempts");
	}

	private TimeSpan CalculateDelay(int attemptNumber)
	{
		if (!_useExponentialBackoff)
		{
			return _retryDelay;
		}

		// Exponential backoff: delay * 2^attempt
		var exponentialDelay = TimeSpan.FromMilliseconds(_retryDelay.TotalMilliseconds * Math.Pow(2, attemptNumber));
		return exponentialDelay > _maxRetryDelay ? _maxRetryDelay : exponentialDelay;
	}

	private static bool IsRetryableStatusCode(HttpStatusCode statusCode)
		=> RetryableStatusCodes.Contains(statusCode);

	private static bool IsRetryableException(Exception ex)
		=> ex is HttpRequestException or TaskCanceledException or SocketException;
}