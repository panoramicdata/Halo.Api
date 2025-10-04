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
				.AddConsole()
				.AddDebug()
				.SetMinimumLevel(LogLevel.Information)
		);

		Logger = loggerFactory.CreateLogger<IntegrationTestFixture>();
	}

	public IHaloClient GetHaloClient()
	{
		if (_haloClient == null)
		{
			var options = new HaloClientOptions
			{
				HaloAccount = Configuration["HaloApi:HaloAccount"] ?? throw new InvalidOperationException("HaloApi:HaloAccount not found in user secrets"),
				HaloClientId = Configuration["HaloApi:HaloClientId"] ?? throw new InvalidOperationException("HaloApi:HaloClientId not found in user secrets"),
				HaloClientSecret = Configuration["HaloApi:HaloClientSecret"] ?? throw new InvalidOperationException("HaloApi:HaloClientSecret not found in user secrets")
			};

			_haloClient = new HaloClient(options);
			Logger.LogInformation("Successfully created HaloClient for account: {Account}", options.HaloAccount);
		}

		return _haloClient;
	}

	public void Dispose()
	{
		// Cleanup if needed
		_haloClient = null;
		Logger.LogInformation("IntegrationTestFixture disposed");
		GC.SuppressFinalize(this);
	}
}