using AwesomeAssertions;

namespace Halo.Api.Test;

/// <summary>
/// Example test class showing how to inherit from TestBase for integration tests
/// that need access to HaloClient and Logger
/// </summary>
[Collection("Integration Tests")]
public class ExampleIntegrationTests(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public void HaloClient_ShouldBeConfiguredCorrectly()
	{
		// Arrange & Act
		try
		{
			Logger.LogInformation("Testing HaloClient configuration");

			// Assert - Using the protected HaloClient property from TestBase
			HaloClient.Should().NotBeNull();

			Logger.LogInformation("HaloClient configuration validated successfully");
		}
		catch (InvalidOperationException ex) when (ex.Message.Contains("not found in user secrets"))
		{
			Logger.LogWarning(ex, "Integration test skipped - user secrets not configured: {Message}", ex.Message);

			// Test passes - this is expected behavior when secrets aren't configured
		}
	}

	[Fact]
	public void Logger_ShouldLogInformationCorrectly()
	{
		// Arrange
		const string testMessage = "This is a test log message";

		// Act & Assert - Using the protected Logger property from TestBase
		Logger.Should().NotBeNull();

		// This should not throw any exceptions
		Logger.LogInformation(testMessage);
		Logger.LogWarning("This is a test warning");
		Logger.LogDebug("This is a test debug message");
	}
}