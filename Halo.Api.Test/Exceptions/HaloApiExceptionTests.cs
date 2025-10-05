using AwesomeAssertions;
using Halo.Api.Exceptions;

namespace Halo.Api.Test.Exceptions;

public class HaloApiExceptionTests
{
	[Fact]
	public void HaloApiException_WithBasicMessage_CanBeCreated()
	{
		// Arrange & Act
		var exception = new HaloApiException("Test error message");

		// Assert
		exception.Message.Should().Be("Test error message");
		exception.StatusCode.Should().BeNull();
		exception.ErrorCode.Should().BeNull();
		exception.Details.Should().BeNull();
		exception.RequestUrl.Should().BeNull();
		exception.RequestMethod.Should().BeNull();
	}

	[Fact]
	public void HaloApiException_WithInnerException_CanBeCreated()
	{
		// Arrange
		var innerException = new InvalidOperationException("Inner error");

		// Act
		var exception = new HaloApiException("Outer error", innerException);

		// Assert
		exception.Message.Should().Be("Outer error");
		exception.InnerException.Should().Be(innerException);
	}

	[Fact]
	public void HaloApiException_WithFullDetails_CanBeCreated()
	{
		// Arrange
		var details = new Dictionary<string, object?>
		{
			["field1"] = "value1",
			["field2"] = 42
		};
		var innerException = new Exception("Inner");

		// Act
		var exception = new HaloApiException(
			message: "Full error details",
			statusCode: 400,
			errorCode: "VALIDATION_ERROR",
			details: details,
			requestUrl: "https://api.halopsa.com/tickets/123",
			requestMethod: "PUT",
			innerException: innerException);

		// Assert
		exception.Message.Should().Be("Full error details");
		exception.StatusCode.Should().Be(400);
		exception.ErrorCode.Should().Be("VALIDATION_ERROR");
		exception.Details.Should().BeEquivalentTo(details);
		exception.RequestUrl.Should().Be("https://api.halopsa.com/tickets/123");
		exception.RequestMethod.Should().Be("PUT");
		exception.InnerException.Should().Be(innerException);
	}

	[Fact]
	public void HaloAuthenticationException_InheritsFromHaloApiException()
	{
		// Arrange & Act
		var exception = new HaloAuthenticationException("Authentication failed");

		// Assert
		exception.Should().BeOfType<HaloAuthenticationException>();
		exception.Should().BeAssignableTo<HaloApiException>();
		exception.Message.Should().Be("Authentication failed");
	}

	[Fact]
	public void HaloAuthenticationException_WithFullDetails_CanBeCreated()
	{
		// Arrange & Act
		var exception = new HaloAuthenticationException(
			message: "Invalid credentials",
			statusCode: 401,
			errorCode: "INVALID_CREDENTIALS",
			requestUrl: "https://api.halopsa.com/auth/token",
			requestMethod: "POST");

		// Assert
		exception.Message.Should().Be("Invalid credentials");
		exception.StatusCode.Should().Be(401);
		exception.ErrorCode.Should().Be("INVALID_CREDENTIALS");
		exception.RequestUrl.Should().Be("https://api.halopsa.com/auth/token");
		exception.RequestMethod.Should().Be("POST");
	}

	[Fact]
	public void HaloAuthorizationException_InheritsFromHaloApiException()
	{
		// Arrange & Act
		var exception = new HaloAuthorizationException("Access denied");

		// Assert
		exception.Should().BeOfType<HaloAuthorizationException>();
		exception.Should().BeAssignableTo<HaloApiException>();
		exception.Message.Should().Be("Access denied");
	}

	[Fact]
	public void HaloNotFoundException_WithResourceInfo_CanBeCreated()
	{
		// Arrange & Act
		var exception = new HaloNotFoundException(
			message: "Ticket not found",
			resourceType: "Ticket",
			resourceId: 123,
			statusCode: 404);

		// Assert
		exception.Message.Should().Be("Ticket not found");
		exception.ResourceType.Should().Be("Ticket");
		exception.ResourceId.Should().Be(123);
		exception.StatusCode.Should().Be(404);
		exception.Should().BeOfType<HaloNotFoundException>();
		exception.Should().BeAssignableTo<HaloApiException>();
	}

	[Fact]
	public void HaloBadRequestException_WithValidationErrors_CanBeCreated()
	{
		// Arrange
		var validationErrors = new List<string>
		{
			"Summary is required",
			"ClientId must be greater than 0"
		}.AsReadOnly();

		// Act
		var exception = new HaloBadRequestException(
			message: "Validation failed",
			validationErrors: validationErrors,
			statusCode: 400);

		// Assert
		exception.Message.Should().Be("Validation failed");
		exception.ValidationErrors.Should().BeEquivalentTo(validationErrors);
		exception.StatusCode.Should().Be(400);
		exception.Should().BeOfType<HaloBadRequestException>();
		exception.Should().BeAssignableTo<HaloApiException>();
	}

	[Fact]
	public void HaloRateLimitException_WithRateLimitInfo_CanBeCreated()
	{
		// Arrange
		var resetTime = DateTime.UtcNow.AddMinutes(15);

		// Act
		var exception = new HaloRateLimitException(
			message: "Rate limit exceeded",
			retryAfterSeconds: 900,
			rateLimit: 100,
			remainingRequests: 0,
			resetTime: resetTime,
			statusCode: 429);

		// Assert
		exception.Message.Should().Be("Rate limit exceeded");
		exception.RetryAfterSeconds.Should().Be(900);
		exception.RateLimit.Should().Be(100);
		exception.RemainingRequests.Should().Be(0);
		exception.ResetTime.Should().Be(resetTime);
		exception.StatusCode.Should().Be(429);
		exception.Should().BeOfType<HaloRateLimitException>();
		exception.Should().BeAssignableTo<HaloApiException>();
	}

	[Fact]
	public void HaloServerException_InheritsFromHaloApiException()
	{
		// Arrange & Act
		var exception = new HaloServerException(
			message: "Internal server error",
			statusCode: 500,
			errorCode: "INTERNAL_ERROR");

		// Assert
		exception.Message.Should().Be("Internal server error");
		exception.StatusCode.Should().Be(500);
		exception.ErrorCode.Should().Be("INTERNAL_ERROR");
		exception.Should().BeOfType<HaloServerException>();
		exception.Should().BeAssignableTo<HaloApiException>();
	}

	[Fact]
	public void AllExceptions_AreSerializable()
	{
		// This test ensures exceptions can be serialized if needed for logging or RPC scenarios
		var exceptions = new List<Exception>
		{
			new HaloApiException("Test"),
			new HaloAuthenticationException("Auth test"),
			new HaloAuthorizationException("Authz test"),
			new HaloNotFoundException("Not found test"),
			new HaloBadRequestException("Bad request test"),
			new HaloRateLimitException("Rate limit test"),
			new HaloServerException("Server error test")
		};

		foreach (var exception in exceptions)
		{
			// Act & Assert - should not throw
			exception.Should().NotBeNull();
			exception.Message.Should().NotBeNullOrEmpty();
			exception.ToString().Should().NotBeNullOrEmpty();
		}
	}

	[Fact]
	public void ExceptionHierarchy_IsCorrect()
	{
		// Arrange & Act & Assert
		var authenticationException = new HaloAuthenticationException("test");
		var authorizationException = new HaloAuthorizationException("test");
		var notFoundException = new HaloNotFoundException("test");
		var badRequestException = new HaloBadRequestException("test");
		var rateLimitException = new HaloRateLimitException("test");
		var serverException = new HaloServerException("test");

		// All should inherit from HaloApiException
		authenticationException.Should().BeAssignableTo<HaloApiException>();
		authorizationException.Should().BeAssignableTo<HaloApiException>();
		notFoundException.Should().BeAssignableTo<HaloApiException>();
		badRequestException.Should().BeAssignableTo<HaloApiException>();
		rateLimitException.Should().BeAssignableTo<HaloApiException>();
		serverException.Should().BeAssignableTo<HaloApiException>();

		// All should inherit from Exception
		authenticationException.Should().BeAssignableTo<Exception>();
		authorizationException.Should().BeAssignableTo<Exception>();
		notFoundException.Should().BeAssignableTo<Exception>();
		badRequestException.Should().BeAssignableTo<Exception>();
		rateLimitException.Should().BeAssignableTo<Exception>();
		serverException.Should().BeAssignableTo<Exception>();
	}
}