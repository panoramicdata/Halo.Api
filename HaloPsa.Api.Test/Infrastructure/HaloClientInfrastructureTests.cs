using AwesomeAssertions;

namespace HaloPsa.Api.Test.Infrastructure;

[Collection("Integration Tests")]
public class HaloClientInfrastructureTests(IntegrationTestFixture fixture)
{
	private readonly IntegrationTestFixture _fixture = fixture;

	[Fact]
	public void HaloClient_WithValidOptions_CanBeInstantiated()
	{
		// Arrange
		var options = new HaloClientOptions
		{
			HaloAccount = _fixture.Configuration["HaloApi:HaloAccount"] ?? throw new InvalidOperationException("HaloApi:HaloAccount not found"),
			HaloClientId = _fixture.Configuration["HaloApi:HaloClientId"] ?? throw new InvalidOperationException("HaloApi:HaloClientId not found"),
			HaloClientSecret = _fixture.Configuration["HaloApi:HaloClientSecret"] ?? throw new InvalidOperationException("HaloApi:HaloClientSecret not found")
		};

		// Act
		using var client = new HaloClient(options);

		// Assert
		_ = client.Should().NotBeNull();
		_ = client.Account.Should().Be(options.HaloAccount);
		_ = client.BaseUrl.Should().NotBeNullOrEmpty();
		_ = client.Psa.Should().NotBeNull();
		_ = client.ServiceDesk.Should().NotBeNull();
		_ = client.System.Should().NotBeNull();
	}

	[Fact]
	public void HaloClient_WithExtendedOptions_CanBeInstantiated()
	{
		// Arrange
		var options = new HaloClientOptions
		{
			HaloAccount = _fixture.Configuration["HaloApi:HaloAccount"] ?? throw new InvalidOperationException("HaloApi:HaloAccount not found"),
			HaloClientId = _fixture.Configuration["HaloApi:HaloClientId"] ?? throw new InvalidOperationException("HaloApi:HaloClientId not found"),
			HaloClientSecret = _fixture.Configuration["HaloApi:HaloClientSecret"] ?? throw new InvalidOperationException("HaloApi:HaloClientSecret not found"),
			RequestTimeout = TimeSpan.FromSeconds(60),
			MaxRetryAttempts = 5,
			RetryDelay = TimeSpan.FromSeconds(2),
			EnableRequestLogging = true,
			EnableResponseLogging = true,
			Logger = _fixture.Logger
		};

		// Act
		using var client = new HaloClient(options);

		// Assert
		_ = client.Should().NotBeNull();
		_ = client.Account.Should().Be(options.HaloAccount);
		_ = client.BaseUrl.Should().Contain(options.HaloAccount);
	}

	[Fact]
	public void HaloClient_WithCustomBaseUrl_UsesCustomUrl()
	{
		// Arrange
		var customBaseUrl = "https://custom.halopsa.com";
		var options = new HaloClientOptions
		{
			HaloAccount = _fixture.Configuration["HaloApi:HaloAccount"] ?? throw new InvalidOperationException("HaloApi:HaloAccount not found"),
			HaloClientId = _fixture.Configuration["HaloApi:HaloClientId"] ?? throw new InvalidOperationException("HaloApi:HaloClientId not found"),
			HaloClientSecret = _fixture.Configuration["HaloApi:HaloClientSecret"] ?? throw new InvalidOperationException("HaloApi:HaloClientSecret not found"),
			BaseUrl = customBaseUrl
		};

		// Act
		using var client = new HaloClient(options);

		// Assert
		_ = client.BaseUrl.Should().Be(customBaseUrl);
	}

	[Fact]
	public void HaloClientOptions_WithInvalidAccount_ThrowsArgumentException()
	{
		// Arrange & Act & Assert
		Action act = () => new HaloClientOptions
		{
			HaloAccount = "",
			HaloClientId = Guid.NewGuid().ToString(),
			HaloClientSecret = $"{Guid.NewGuid()}-{Guid.NewGuid()}"
		}.Validate();

		_ = act.Should().Throw<ArgumentException>()
			.WithMessage("HaloAccount cannot be null or empty.*");
	}

	[Fact]
	public void HaloClientOptions_WithInvalidClientId_ThrowsFormatException()
	{
		// Arrange & Act & Assert
		Action act = () => new HaloClientOptions
		{
			HaloAccount = "test-account",
			HaloClientId = "invalid-guid",
			HaloClientSecret = $"{Guid.NewGuid()}-{Guid.NewGuid()}"
		}.Validate();

		_ = act.Should().Throw<FormatException>()
			.WithMessage("HaloClientId must be a valid GUID format*");
	}

	[Fact]
	public void HaloClientOptions_WithInvalidClientSecret_ThrowsFormatException()
	{
		// Arrange & Act & Assert
		Action act = () => new HaloClientOptions
		{
			HaloAccount = "test-account",
			HaloClientId = Guid.NewGuid().ToString(),
			HaloClientSecret = "invalid-secret"
		}.Validate();

		_ = act.Should().Throw<FormatException>()
			.WithMessage("HaloClientSecret must be in the format of two concatenated GUIDs*");
	}

	[Fact]
	public void HaloClientOptions_WithInvalidTimeout_ThrowsArgumentException()
	{
		// Arrange & Act & Assert
		Action act = () => new HaloClientOptions
		{
			HaloAccount = "test-account",
			HaloClientId = Guid.NewGuid().ToString(),
			HaloClientSecret = $"{Guid.NewGuid()}-{Guid.NewGuid()}",
			RequestTimeout = TimeSpan.Zero
		}.Validate();

		_ = act.Should().Throw<ArgumentException>()
			.WithMessage("RequestTimeout must be greater than zero.*");
	}
}