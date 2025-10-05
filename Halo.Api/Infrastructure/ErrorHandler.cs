using Halo.Api.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Refit;
using System.Text.Json;

namespace Halo.Api.Infrastructure;

/// <summary>
/// HTTP message handler that converts Refit API exceptions to HaloApiExceptions
/// </summary>
internal sealed class ErrorHandler(ILogger? logger) : DelegatingHandler
{
	private readonly ILogger _logger = logger ?? NullLogger.Instance;

	/// <summary>
	/// Processes HTTP requests and converts any API exceptions to HaloApiExceptions
	/// </summary>
	/// <param name="request">The HTTP request message</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The HTTP response message</returns>
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		try
		{
			var response = await base.SendAsync(request, cancellationToken);
			return response;
		}
		catch (ApiException apiException)
		{
			_logger.LogError(apiException, "API exception occurred: {StatusCode} {ReasonPhrase}",
				apiException.StatusCode, apiException.ReasonPhrase);

			// Convert Refit ApiException to appropriate HaloApiException
			var haloException = ConvertToHaloApiException(apiException, request);
			throw haloException;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unexpected error occurred during HTTP request to {RequestUri}", request.RequestUri);
			throw;
		}
	}

	/// <summary>
	/// Converts a Refit ApiException to the appropriate HaloApiException type
	/// </summary>
	/// <param name="apiException">The Refit API exception</param>
	/// <param name="request">The original HTTP request</param>
	/// <returns>The appropriate HaloApiException</returns>
	private static HaloApiException ConvertToHaloApiException(ApiException apiException, HttpRequestMessage request)
	{
		var statusCode = (int)apiException.StatusCode;
		var message = apiException.ReasonPhrase ?? $"API request failed with status {statusCode}";
		var requestUrl = request.RequestUri?.ToString();
		var requestMethod = request.Method.Method;

		// Try to parse error details from response content
		Dictionary<string, object?>? details = null;
		IReadOnlyList<string>? validationErrors = null;

		if (!string.IsNullOrEmpty(apiException.Content))
		{
			try
			{
				var jsonDoc = JsonDocument.Parse(apiException.Content);
				details = ExtractErrorDetails(jsonDoc.RootElement);

				// Extract validation errors if present
				if (jsonDoc.RootElement.TryGetProperty("errors", out var errorsElement))
				{
					var errorsList = new List<string>();
					if (errorsElement.ValueKind == JsonValueKind.Array)
					{
						foreach (var error in errorsElement.EnumerateArray())
						{
							if (error.ValueKind == JsonValueKind.String)
							{
								errorsList.Add(error.GetString() ?? string.Empty);
							}
						}
					}

					validationErrors = errorsList.AsReadOnly();
				}
			}
			catch (JsonException)
			{
				// If we can't parse the JSON, just use the raw content
				details = new Dictionary<string, object?> { ["rawContent"] = apiException.Content };
			}
		}

		// Map status codes to specific exception types
		return statusCode switch
		{
			400 => new HaloBadRequestException(
				message: $"Bad request: {message}",
				validationErrors: validationErrors,
				statusCode: statusCode,
				errorCode: null,
				details: details,
				requestUrl: requestUrl,
				requestMethod: requestMethod,
				innerException: apiException),

			401 => new HaloAuthenticationException(
				message: $"Authentication failed: {message}",
				statusCode: statusCode,
				errorCode: null,
				details: details,
				requestUrl: requestUrl,
				requestMethod: requestMethod,
				innerException: apiException),

			403 => new HaloAuthorizationException(
				message: $"Authorization failed: {message}",
				statusCode: statusCode,
				errorCode: null,
				details: details,
				requestUrl: requestUrl,
				requestMethod: requestMethod,
				innerException: apiException),

			404 => new HaloNotFoundException(
				message: $"Resource not found: {message}",
				resourceType: ExtractResourceTypeFromUrl(requestUrl),
				resourceId: ExtractResourceIdFromUrl(requestUrl),
				statusCode: statusCode,
				errorCode: null,
				details: details,
				requestUrl: requestUrl,
				requestMethod: requestMethod,
				innerException: apiException),

			429 => new HaloRateLimitException(
				message: $"Rate limit exceeded: {message}",
				retryAfterSeconds: ExtractRetryAfterSeconds(apiException),
				rateLimit: null,
				remainingRequests: null,
				resetTime: null,
				statusCode: statusCode,
				errorCode: null,
				details: details,
				requestUrl: requestUrl,
				requestMethod: requestMethod,
				innerException: apiException),

			>= 500 => new HaloServerException(
				message: $"Server error: {message}",
				statusCode: statusCode,
				errorCode: null,
				details: details,
				requestUrl: requestUrl,
				requestMethod: requestMethod,
				innerException: apiException),

			_ => new HaloApiException(
				message: $"API error: {message}",
				statusCode: statusCode,
				errorCode: null,
				details: details,
				requestUrl: requestUrl,
				requestMethod: requestMethod,
				innerException: apiException)
		};
	}

	/// <summary>
	/// Extracts error details from JSON response
	/// </summary>
	/// <param name="element">The JSON element to extract from</param>
	/// <returns>Dictionary of error details</returns>
	private static Dictionary<string, object?> ExtractErrorDetails(JsonElement element)
	{
		var details = new Dictionary<string, object?>();

		foreach (var property in element.EnumerateObject())
		{
			details[property.Name] = property.Value.ValueKind switch
			{
				JsonValueKind.String => property.Value.GetString(),
				JsonValueKind.Number => property.Value.TryGetInt32(out var intVal) ? intVal : property.Value.GetDouble(),
				JsonValueKind.True => true,
				JsonValueKind.False => false,
				JsonValueKind.Null => null,
				_ => property.Value.GetRawText()
			};
		}

		return details;
	}

	/// <summary>
	/// Extracts resource type from URL (e.g., "Tickets", "Users", "Clients")
	/// </summary>
	/// <param name="url">The request URL</param>
	/// <returns>The resource type or null if not found</returns>
	private static string? ExtractResourceTypeFromUrl(string? url)
	{
		if (string.IsNullOrEmpty(url))
		{
			return null;
		}

		var uri = new Uri(url);
		var segments = uri.Segments;

		// Look for /api/{resourceType} pattern
		for (var i = 0; i < segments.Length - 1; i++)
		{
			if (segments[i].Equals("api/", StringComparison.OrdinalIgnoreCase))
			{
				var resourceSegment = segments[i + 1].TrimEnd('/');
				return resourceSegment;
			}
		}

		return null;
	}

	/// <summary>
	/// Extracts resource ID from URL (e.g., the ID in /api/Tickets/123)
	/// </summary>
	/// <param name="url">The request URL</param>
	/// <returns>The resource ID or null if not found</returns>
	private static object? ExtractResourceIdFromUrl(string? url)
	{
		if (string.IsNullOrEmpty(url))
		{
			return null;
		}

		var uri = new Uri(url);
		var segments = uri.Segments;

		// Look for /api/{resourceType}/{id} pattern
		for (var i = 0; i < segments.Length - 2; i++)
		{
			if (segments[i].Equals("api/", StringComparison.OrdinalIgnoreCase))
			{
				var idSegment = segments[i + 2].TrimEnd('/');
				return int.TryParse(idSegment, out var intId) ? intId : idSegment;
			}
		}

		return null;
	}

	/// <summary>
	/// Extracts Retry-After header value in seconds
	/// </summary>
	/// <param name="apiException">The API exception</param>
	/// <returns>Retry-After value in seconds or null</returns>
	private static int? ExtractRetryAfterSeconds(ApiException apiException)
	{
		if (apiException.Headers?.TryGetValues("Retry-After", out var values) == true)
		{
			var retryAfter = values.FirstOrDefault();
			if (int.TryParse(retryAfter, out var seconds))
			{
				return seconds;
			}
		}

		return null;
	}
}