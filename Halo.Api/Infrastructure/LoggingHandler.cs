using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Halo.Api.Infrastructure;

/// <summary>
/// HTTP message handler that provides logging capabilities for requests and responses
/// </summary>
internal sealed class LoggingHandler(ILogger? logger, bool logRequests, bool logResponses) : DelegatingHandler
{
	private readonly ILogger? _logger = logger;
	private readonly bool _logRequests = logRequests;
	private readonly bool _logResponses = logResponses;

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		var stopwatch = Stopwatch.StartNew();
		var requestId = Guid.NewGuid().ToString("N")[..8];

		if (_logRequests && _logger != null)
		{
			LogHttpRequest(request, requestId);
		}

		HttpResponseMessage? response = null;
		Exception? exception = null;

		try
		{
			response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
			return response;
		}
		catch (Exception ex)
		{
			exception = ex;
			throw;
		}
		finally
		{
			stopwatch.Stop();

			if (_logger != null)
			{
				if (exception != null)
				{
					LogHttpException(request, exception, stopwatch.Elapsed, requestId);
				}
				else if (_logResponses && response != null)
				{
					LogHttpResponse(response, stopwatch.Elapsed, requestId);
				}
			}
		}
	}

	private void LogHttpRequest(HttpRequestMessage request, string requestId)
	{
#pragma warning disable CA1848 // Use LoggerMessage delegates for better performance
		_logger?.LogInformation(
			"[{RequestId}] HTTP {Method} {Uri}",
			requestId,
			request.Method,
			request.RequestUri);

		if (request.Content != null)
		{
			_logger?.LogDebug(
				"[{RequestId}] Request Headers: {Headers}",
				requestId,
				string.Join(", ", request.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}")));
		}
#pragma warning restore CA1848
	}

	private void LogHttpResponse(HttpResponseMessage response, TimeSpan elapsed, string requestId)
	{
		var level = response.IsSuccessStatusCode ? LogLevel.Information : LogLevel.Warning;

#pragma warning disable CA1848 // Use LoggerMessage delegates for better performance
		_logger?.Log(level,
			"[{RequestId}] HTTP {StatusCode} {ReasonPhrase} in {ElapsedMs}ms",
			requestId,
			(int)response.StatusCode,
			response.ReasonPhrase,
			elapsed.TotalMilliseconds);

		if (!response.IsSuccessStatusCode)
		{
			_logger?.LogDebug(
				"[{RequestId}] Response Headers: {Headers}",
				requestId,
				string.Join(", ", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}")));
		}
#pragma warning restore CA1848
	}

	private void LogHttpException(HttpRequestMessage request, Exception exception, TimeSpan elapsed, string requestId)
	{
#pragma warning disable CA1848 // Use LoggerMessage delegates for better performance
		_logger?.LogError(exception,
			"[{RequestId}] HTTP {Method} {Uri} failed after {ElapsedMs}ms: {ErrorMessage}",
			requestId,
			request.Method,
			request.RequestUri,
			elapsed.TotalMilliseconds,
			exception.Message);
#pragma warning restore CA1848
	}
}