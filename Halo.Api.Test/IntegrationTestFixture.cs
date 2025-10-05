using Halo.Api.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Halo.Api.Test;

public class IntegrationTestFixture : IDisposable
{
	public IConfiguration Configuration { get; }
	public ILogger Logger { get; }

	private IHaloClient? _haloClient;

	public IntegrationTestFixture()
	{
		// Build configuration to read from user secrets
		Configuration = new ConfigurationBuilder()
			.AddUserSecrets<IntegrationTestFixture>()
			.Build();

		// Create logger factory and logger
		var loggerFactory = LoggerFactory.Create(builder =>
			builder
				.SetMinimumLevel(LogLevel.Information)
		);

		Logger = loggerFactory.CreateLogger<IntegrationTestFixture>();

		// Validate user secrets are present immediately
		ValidateUserSecrets();
	}

	private void ValidateUserSecrets()
	{
		var haloAccount = Configuration["HaloApi:HaloAccount"];
		var haloClientId = Configuration["HaloApi:HaloClientId"];
		var haloClientSecret = Configuration["HaloApi:HaloClientSecret"];

		if (string.IsNullOrWhiteSpace(haloAccount))
		{
			throw new InvalidOperationException(
				"HaloApi:HaloAccount not found in user secrets. " +
				"Please run: dotnet user-secrets set \"HaloApi:HaloAccount\" \"your-account-name\" --project Halo.Api.Test");
		}

		if (string.IsNullOrWhiteSpace(haloClientId))
		{
			throw new InvalidOperationException(
				"HaloApi:HaloClientId not found in user secrets. " +
				"Please run: dotnet user-secrets set \"HaloApi:HaloClientId\" \"your-client-id\" --project Halo.Api.Test");
		}

		if (string.IsNullOrWhiteSpace(haloClientSecret))
		{
			throw new InvalidOperationException(
				"HaloApi:HaloClientSecret not found in user secrets. " +
				"Please run: dotnet user-secrets set \"HaloApi:HaloClientSecret\" \"your-client-secret\" --project Halo.Api.Test");
		}

		Logger.LogInformation("User secrets validation passed for account: {Account}", haloAccount);
	}

	public IHaloClient GetHaloClient()
	{
		if (_haloClient == null)
		{
			var options = new HaloClientOptions
			{
				HaloAccount = Configuration["HaloApi:HaloAccount"]!,
				HaloClientId = Configuration["HaloApi:HaloClientId"]!,
				HaloClientSecret = Configuration["HaloApi:HaloClientSecret"]!,
				Logger = Logger,
				EnableRequestLogging = true,
				EnableResponseLogging = true
			};

			_haloClient = new HaloClient(options);
			Logger.LogInformation("Successfully created HaloClient for account: {Account}", options.HaloAccount);
		}

		return _haloClient;
	}

	public void Dispose()
	{
		// Cleanup if needed
		if (_haloClient is IDisposable disposableClient)
		{
			disposableClient.Dispose();
		}

		_haloClient = null;
		Logger.LogInformation("IntegrationTestFixture disposed");
		GC.SuppressFinalize(this);
	}
}