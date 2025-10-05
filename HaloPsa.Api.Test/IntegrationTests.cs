using AwesomeAssertions;

namespace HaloPsa.Api.Test;

[Collection("Integration Tests")]
public class IntegrationTests(IntegrationTestFixture fixture, ITestOutputHelper output) : TestBase(fixture)
{
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void InjectedHaloClient_ShouldNotBeNull()
	{
		// Arrange & Act
		try
		{
			// Assert
			_ = HaloClient.Should().NotBeNull();


			Logger.LogInformation("Successfully validated IHaloClient instance from user secrets");
		}
		catch (InvalidOperationException ex) when (ex.Message.Contains("not found in user secrets"))
		{
			// This is expected when user secrets are not configured
			Logger.LogWarning(ex, "Integration test skipped - user secrets not configured: {Message}", ex.Message);
			// Test passes - this is the expected behavior when secrets aren't configured
		}
	}

	[Fact]
	public void Configuration_ShouldNotBeNull()
	{
		// Arrange & Act
		var configuration = _fixture.Configuration;

		// Assert
		_ = configuration.Should().NotBeNull();
		Logger.LogInformation("Configuration successfully validated");
		_output.WriteLine("Configuration successfully initialized");
	}

	[Fact]
	public void Logger_ShouldBeAvailable()
	{
		// Arrange & Act & Assert
		_ = Logger.Should().NotBeNull();
		Logger.LogInformation("Logger functionality test");
		_output.WriteLine("Logger successfully validated and tested");
	}
}