using Halo.Api.Exceptions;
using Halo.Api.Infrastructure;
using Halo.Api.Interfaces;
using Refit;
using System.Text.Json;

namespace Halo.Api;

/// <summary>
/// Implementation of PSA API module
/// </summary>
internal sealed class PsaApi(HttpClient _httpClient) : IPsaApi
{
	private static readonly RefitSettings _refitSettings = new()
	{
		ExceptionFactory = ConvertApiExceptionToHaloApiException
	};

	public TicketsApiWrapper Tickets { get; } = new Lazy<TicketsApiWrapper>(() => new TicketsApiWrapper(RestService.For<ITicketsApi>(_httpClient, _refitSettings))).Value;
	public TicketTypesApiWrapper TicketTypes { get; } = new Lazy<TicketTypesApiWrapper>(() => new TicketTypesApiWrapper(RestService.For<ITicketTypesRefitApi>(_httpClient, _refitSettings))).Value;
	public UsersApiWrapper Users { get; } = new Lazy<UsersApiWrapper>(() => new UsersApiWrapper(RestService.For<IUsersRefitApi>(_httpClient, _refitSettings))).Value;
	public ClientsApiWrapper Clients { get; } = new Lazy<ClientsApiWrapper>(() => new ClientsApiWrapper(RestService.For<IClientsRefitApi>(_httpClient, _refitSettings))).Value;
	public AssetsApiWrapper Assets { get; } = new Lazy<AssetsApiWrapper>(() => new AssetsApiWrapper(RestService.For<IAssetsRefitApi>(_httpClient, _refitSettings))).Value;
	public ProjectsApiWrapper Projects { get; } = new Lazy<ProjectsApiWrapper>(() => new ProjectsApiWrapper(RestService.For<IProjectsRefitApi>(_httpClient, _refitSettings))).Value;

	/// <summary>
	/// Converts Refit ApiExceptions to appropriate HaloApiExceptions
	/// </summary>
	/// <param name="httpResponseMessage">The HTTP response message</param>
	/// <returns>The appropriate HaloApiException or null if no exception should be thrown</returns>
	private static async Task<Exception?> ConvertApiExceptionToHaloApiException(HttpResponseMessage httpResponseMessage)
	{
		if (httpResponseMessage.IsSuccessStatusCode)
		{
			return null; // No exception for successful responses
		}

		var statusCode = (int)httpResponseMessage.StatusCode;
		var message = httpResponseMessage.ReasonPhrase ?? $"API request failed with status {statusCode}";
		var requestUrl = httpResponseMessage.RequestMessage?.RequestUri?.ToString();
		var requestMethod = httpResponseMessage.RequestMessage?.Method.Method;

		// Try to parse error details from response content
		Dictionary<string, object?>? details = null;
		IReadOnlyList<string>? validationErrors = null;
		string? content = null;

		try
		{
			content = await httpResponseMessage.Content.ReadAsStringAsync();
			if (!string.IsNullOrEmpty(content))
			{
				var jsonDoc = JsonDocument.Parse(content);
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
		}
		catch (JsonException)
		{
			// If we can't parse the JSON, just use the raw content
			details = new Dictionary<string, object?> { ["rawContent"] = content };
		}

		// Map status codes to specific exception types
		return statusCode switch
		{
			400 => new HaloBadRequestException(
				message: $"Bad request: {message}",
				validationErrors: validationErrors,
				statusCode: statusCode,
				details: details,
				requestUrl: requestUrl,
				requestMethod: requestMethod),

			401 => new HaloAuthenticationException(
				message: $"Authentication failed: {message}",
				statusCode: statusCode,
				details: details,
				requestUrl: requestUrl,
				requestMethod: requestMethod),

			403 => new HaloAuthorizationException(
				message: $"Authorization failed: {message}",
				statusCode: statusCode,
				details: details,
				requestUrl: requestUrl,
				requestMethod: requestMethod),

			404 => new HaloNotFoundException(
				message: $"Resource not found: {message}",
				resourceType: ExtractResourceTypeFromUrl(requestUrl),
				resourceId: ExtractResourceIdFromUrl(requestUrl),
				statusCode: statusCode,
				details: details,
				requestUrl: requestUrl,
				requestMethod: requestMethod),

			429 => new HaloRateLimitException(
				message: $"Rate limit exceeded: {message}",
				retryAfterSeconds: ExtractRetryAfterSeconds(httpResponseMessage),
				statusCode: statusCode,
				details: details,
				requestUrl: requestUrl,
				requestMethod: requestMethod),

			>= 500 => new HaloServerException(
				message: $"Server error: {message}",
				statusCode: statusCode,
				details: details,
				requestUrl: requestUrl,
				requestMethod: requestMethod),

			_ => new HaloApiException(
				message: $"API error: {message}",
				statusCode: statusCode,
				details: details,
				requestUrl: requestUrl,
				requestMethod: requestMethod)
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
		if (string.IsNullOrEmpty(url)) return null;

		var uri = new Uri(url);
		var segments = uri.Segments;

		// Look for /api/{resourceType} pattern
		for (int i = 0; i < segments.Length - 1; i++)
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
		if (string.IsNullOrEmpty(url)) return null;

		var uri = new Uri(url);
		var segments = uri.Segments;

		// Look for /api/{resourceType}/{id} pattern
		for (int i = 0; i < segments.Length - 2; i++)
		{
			if (segments[i].Equals("api/", StringComparison.OrdinalIgnoreCase))
			{
				var idSegment = segments[i + 2].TrimEnd('/');
				if (int.TryParse(idSegment, out var intId))
				{
					return intId;
				}

				return idSegment;
			}
		}

		return null;
	}

	/// <summary>
	/// Extracts Retry-After header value in seconds
	/// </summary>
	/// <param name="httpResponseMessage">The HTTP response message</param>
	/// <returns>Retry-After value in seconds or null</returns>
	private static int? ExtractRetryAfterSeconds(HttpResponseMessage httpResponseMessage)
	{
		if (httpResponseMessage.Headers?.TryGetValues("Retry-After", out var values) == true)
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
