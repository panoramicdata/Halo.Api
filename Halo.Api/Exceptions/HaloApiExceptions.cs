namespace Halo.Api.Exceptions;

/// <summary>
/// Base exception for all Halo API related errors
/// </summary>
public class HaloApiException : Exception
{
	/// <summary>
	/// The HTTP status code associated with the error
	/// </summary>
	public int? StatusCode { get; }

	/// <summary>
	/// The error code from the API response
	/// </summary>
	public string? ErrorCode { get; }

	/// <summary>
	/// Additional error details from the API
	/// </summary>
	public Dictionary<string, object?>? Details { get; }

	/// <summary>
	/// The request URL that caused the error
	/// </summary>
	public string? RequestUrl { get; }

	/// <summary>
	/// The request method that caused the error
	/// </summary>
	public string? RequestMethod { get; }

	/// <summary>
	/// Initializes a new instance of the HaloApiException class with a specified error message
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	public HaloApiException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the HaloApiException class with a specified error message and inner exception
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	/// <param name="innerException">The exception that is the cause of the current exception</param>
	public HaloApiException(string message, Exception innerException) : base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the HaloApiException class with detailed error information
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	/// <param name="statusCode">The HTTP status code associated with the error</param>
	/// <param name="errorCode">The error code from the API response</param>
	/// <param name="details">Additional error details from the API</param>
	/// <param name="requestUrl">The request URL that caused the error</param>
	/// <param name="requestMethod">The request method that caused the error</param>
	/// <param name="innerException">The exception that is the cause of the current exception</param>
	public HaloApiException(
		string message,
		int? statusCode = null,
		string? errorCode = null,
		Dictionary<string, object?>? details = null,
		string? requestUrl = null,
		string? requestMethod = null,
		Exception? innerException = null)
		: base(message, innerException)
	{
		StatusCode = statusCode;
		ErrorCode = errorCode;
		Details = details;
		RequestUrl = requestUrl;
		RequestMethod = requestMethod;
	}
}

/// <summary>
/// Exception thrown when authentication fails
/// </summary>
public class HaloAuthenticationException : HaloApiException
{
	/// <summary>
	/// Initializes a new instance of the HaloAuthenticationException class with a specified error message
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	public HaloAuthenticationException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the HaloAuthenticationException class with a specified error message and inner exception
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	/// <param name="innerException">The exception that is the cause of the current exception</param>
	public HaloAuthenticationException(string message, Exception innerException) : base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the HaloAuthenticationException class with detailed error information
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	/// <param name="statusCode">The HTTP status code associated with the error</param>
	/// <param name="errorCode">The error code from the API response</param>
	/// <param name="details">Additional error details from the API</param>
	/// <param name="requestUrl">The request URL that caused the error</param>
	/// <param name="requestMethod">The request method that caused the error</param>
	/// <param name="innerException">The exception that is the cause of the current exception</param>
	public HaloAuthenticationException(
		string message,
		int? statusCode = null,
		string? errorCode = null,
		Dictionary<string, object?>? details = null,
		string? requestUrl = null,
		string? requestMethod = null,
		Exception? innerException = null)
		: base(message, statusCode, errorCode, details, requestUrl, requestMethod, innerException)
	{
	}
}

/// <summary>
/// Exception thrown when authorization fails (403 Forbidden)
/// </summary>
public class HaloAuthorizationException : HaloApiException
{
	/// <summary>
	/// Initializes a new instance of the HaloAuthorizationException class with a specified error message
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	public HaloAuthorizationException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the HaloAuthorizationException class with a specified error message and inner exception
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	/// <param name="innerException">The exception that is the cause of the current exception</param>
	public HaloAuthorizationException(string message, Exception innerException) : base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the HaloAuthorizationException class with detailed error information
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	/// <param name="statusCode">The HTTP status code associated with the error</param>
	/// <param name="errorCode">The error code from the API response</param>
	/// <param name="details">Additional error details from the API</param>
	/// <param name="requestUrl">The request URL that caused the error</param>
	/// <param name="requestMethod">The request method that caused the error</param>
	/// <param name="innerException">The exception that is the cause of the current exception</param>
	public HaloAuthorizationException(
		string message,
		int? statusCode = null,
		string? errorCode = null,
		Dictionary<string, object?>? details = null,
		string? requestUrl = null,
		string? requestMethod = null,
		Exception? innerException = null)
		: base(message, statusCode, errorCode, details, requestUrl, requestMethod, innerException)
	{
	}
}

/// <summary>
/// Exception thrown when a requested resource is not found (404 Not Found)
/// </summary>
public class HaloNotFoundException : HaloApiException
{
	/// <summary>
	/// The type of resource that was not found
	/// </summary>
	public string? ResourceType { get; }

	/// <summary>
	/// The ID of the resource that was not found
	/// </summary>
	public object? ResourceId { get; }

	/// <summary>
	/// Initializes a new instance of the HaloNotFoundException class with a specified error message
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	public HaloNotFoundException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the HaloNotFoundException class with a specified error message and inner exception
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	/// <param name="innerException">The exception that is the cause of the current exception</param>
	public HaloNotFoundException(string message, Exception innerException) : base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the HaloNotFoundException class with detailed error information
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	/// <param name="resourceType">The type of resource that was not found</param>
	/// <param name="resourceId">The ID of the resource that was not found</param>
	/// <param name="statusCode">The HTTP status code associated with the error</param>
	/// <param name="errorCode">The error code from the API response</param>
	/// <param name="details">Additional error details from the API</param>
	/// <param name="requestUrl">The request URL that caused the error</param>
	/// <param name="requestMethod">The request method that caused the error</param>
	/// <param name="innerException">The exception that is the cause of the current exception</param>
	public HaloNotFoundException(
		string message,
		string? resourceType = null,
		object? resourceId = null,
		int? statusCode = null,
		string? errorCode = null,
		Dictionary<string, object?>? details = null,
		string? requestUrl = null,
		string? requestMethod = null,
		Exception? innerException = null)
		: base(message, statusCode, errorCode, details, requestUrl, requestMethod, innerException)
	{
		ResourceType = resourceType;
		ResourceId = resourceId;
	}
}

/// <summary>
/// Exception thrown when the request is malformed or invalid (400 Bad Request)
/// </summary>
public class HaloBadRequestException : HaloApiException
{
	/// <summary>
	/// Validation errors from the API
	/// </summary>
	public IReadOnlyList<string>? ValidationErrors { get; }

	/// <summary>
	/// Initializes a new instance of the HaloBadRequestException class with a specified error message
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	public HaloBadRequestException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the HaloBadRequestException class with a specified error message and inner exception
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	/// <param name="innerException">The exception that is the cause of the current exception</param>
	public HaloBadRequestException(string message, Exception innerException) : base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the HaloBadRequestException class with detailed error information
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	/// <param name="validationErrors">Validation errors from the API</param>
	/// <param name="statusCode">The HTTP status code associated with the error</param>
	/// <param name="errorCode">The error code from the API response</param>
	/// <param name="details">Additional error details from the API</param>
	/// <param name="requestUrl">The request URL that caused the error</param>
	/// <param name="requestMethod">The request method that caused the error</param>
	/// <param name="innerException">The exception that is the cause of the current exception</param>
	public HaloBadRequestException(
		string message,
		IReadOnlyList<string>? validationErrors = null,
		int? statusCode = null,
		string? errorCode = null,
		Dictionary<string, object?>? details = null,
		string? requestUrl = null,
		string? requestMethod = null,
		Exception? innerException = null)
		: base(message, statusCode, errorCode, details, requestUrl, requestMethod, innerException)
	{
		ValidationErrors = validationErrors;
	}
}

/// <summary>
/// Exception thrown when rate limiting is enforced (429 Too Many Requests)
/// </summary>
public class HaloRateLimitException : HaloApiException
{
	/// <summary>
	/// Number of seconds to wait before retrying
	/// </summary>
	public int? RetryAfterSeconds { get; }

	/// <summary>
	/// The rate limit that was exceeded
	/// </summary>
	public int? RateLimit { get; }

	/// <summary>
	/// Remaining requests in the current rate limit window
	/// </summary>
	public int? RemainingRequests { get; }

	/// <summary>
	/// When the rate limit window resets
	/// </summary>
	public DateTime? ResetTime { get; }

	/// <summary>
	/// Initializes a new instance of the HaloRateLimitException class with a specified error message
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	public HaloRateLimitException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the HaloRateLimitException class with a specified error message and inner exception
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	/// <param name="innerException">The exception that is the cause of the current exception</param>
	public HaloRateLimitException(string message, Exception innerException) : base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the HaloRateLimitException class with detailed error information
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	/// <param name="retryAfterSeconds">Number of seconds to wait before retrying</param>
	/// <param name="rateLimit">The rate limit that was exceeded</param>
	/// <param name="remainingRequests">Remaining requests in the current rate limit window</param>
	/// <param name="resetTime">When the rate limit window resets</param>
	/// <param name="statusCode">The HTTP status code associated with the error</param>
	/// <param name="errorCode">The error code from the API response</param>
	/// <param name="details">Additional error details from the API</param>
	/// <param name="requestUrl">The request URL that caused the error</param>
	/// <param name="requestMethod">The request method that caused the error</param>
	/// <param name="innerException">The exception that is the cause of the current exception</param>
	public HaloRateLimitException(
		string message,
		int? retryAfterSeconds = null,
		int? rateLimit = null,
		int? remainingRequests = null,
		DateTime? resetTime = null,
		int? statusCode = null,
		string? errorCode = null,
		Dictionary<string, object?>? details = null,
		string? requestUrl = null,
		string? requestMethod = null,
		Exception? innerException = null)
		: base(message, statusCode, errorCode, details, requestUrl, requestMethod, innerException)
	{
		RetryAfterSeconds = retryAfterSeconds;
		RateLimit = rateLimit;
		RemainingRequests = remainingRequests;
		ResetTime = resetTime;
	}
}

/// <summary>
/// Exception thrown when the server encounters an internal error (500 Internal Server Error)
/// </summary>
public class HaloServerException : HaloApiException
{
	/// <summary>
	/// Initializes a new instance of the HaloServerException class with a specified error message
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	public HaloServerException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the HaloServerException class with a specified error message and inner exception
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	/// <param name="innerException">The exception that is the cause of the current exception</param>
	public HaloServerException(string message, Exception innerException) : base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the HaloServerException class with detailed error information
	/// </summary>
	/// <param name="message">The message that describes the error</param>
	/// <param name="statusCode">The HTTP status code associated with the error</param>
	/// <param name="errorCode">The error code from the API response</param>
	/// <param name="details">Additional error details from the API</param>
	/// <param name="requestUrl">The request URL that caused the error</param>
	/// <param name="requestMethod">The request method that caused the error</param>
	/// <param name="innerException">The exception that is the cause of the current exception</param>
	public HaloServerException(
		string message,
		int? statusCode = null,
		string? errorCode = null,
		Dictionary<string, object?>? details = null,
		string? requestUrl = null,
		string? requestMethod = null,
		Exception? innerException = null)
		: base(message, statusCode, errorCode, details, requestUrl, requestMethod, innerException)
	{
	}
}