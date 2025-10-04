using Halo.Api.Infrastructure;
using Halo.Api.Interfaces;

namespace Halo.Api;

/// <summary>
/// Client for interacting with the Halo API
/// </summary>
public class HaloClient : IHaloClient, IDisposable
{
	private readonly HaloClientOptions _options;
	private readonly HttpClient _httpClient;
	private readonly Lazy<IPsaApi> _psa;
	private readonly Lazy<IServiceDeskApi> _serviceDesk;
	private readonly Lazy<ISystemApi> _system;
	private bool _disposed;

	/// <summary>
	/// Initializes a new instance of the HaloClient
	/// </summary>
	/// <param name="options">Configuration options for the client</param>
	public HaloClient(HaloClientOptions options)
	{
		ArgumentNullException.ThrowIfNull(options);
		options.Validate();

		_options = options;
		Account = options.HaloAccount;
		_httpClient = CreateHttpClient();

		// Initialize API modules lazily
		_psa = new Lazy<IPsaApi>(() => new PsaApi(_httpClient));
		_serviceDesk = new Lazy<IServiceDeskApi>(() => new ServiceDeskApi(_httpClient));
		_system = new Lazy<ISystemApi>(() => new SystemApi(_httpClient));
	}

	/// <summary>
	/// Gets the PSA (Professional Services Automation) API module
	/// </summary>
	public IPsaApi Psa => _psa.Value;

	/// <summary>
	/// Gets the ServiceDesk API module
	/// </summary>
	public IServiceDeskApi ServiceDesk => _serviceDesk.Value;

	/// <summary>
	/// Gets the System API module for configuration and administration
	/// </summary>
	public ISystemApi System => _system.Value;

	/// <summary>
	/// Gets the Halo account identifier
	/// </summary>
	public string Account { get; }

	/// <summary>
	/// Gets the base URL for the Halo API
	/// </summary>
	public string BaseUrl => _options.EffectiveBaseUrl;

	private HttpClient CreateHttpClient()
	{
		var handler = new HttpClientHandler();

		// Build the handler chain (order matters - authentication should be innermost)
		DelegatingHandler chain = new AuthenticationHandler(_options);

		if (_options.MaxRetryAttempts > 0)
		{
			var retryHandler = new RetryHandler(
				_options.MaxRetryAttempts,
				_options.RetryDelay,
				_options.UseExponentialBackoff,
				_options.MaxRetryDelay,
				_options.Logger);
			retryHandler.InnerHandler = chain;
			chain = retryHandler;
		}

		if (_options.EnableRequestLogging || _options.EnableResponseLogging)
		{
			var loggingHandler = new LoggingHandler(
				_options.Logger,
				_options.EnableRequestLogging,
				_options.EnableResponseLogging);
			loggingHandler.InnerHandler = chain;
			chain = loggingHandler;
		}

		// Set the innermost handler
		chain.InnerHandler = handler;

		var httpClient = new HttpClient(chain)
		{
			BaseAddress = new Uri(_options.EffectiveBaseUrl),
			Timeout = _options.RequestTimeout
		};

		// Add default headers
		httpClient.DefaultRequestHeaders.Add("User-Agent", "Halo.Api/.NET");
		foreach (var header in _options.DefaultHeaders)
		{
			httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
		}

		return httpClient;
	}

	/// <summary>
	/// Disposes the HTTP client and releases resources
	/// </summary>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Protected dispose method
	/// </summary>
	/// <param name="disposing">True if disposing managed resources</param>
	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			_httpClient?.Dispose();
			_disposed = true;
		}
	}
}

/// <summary>
/// Implementation of PSA API module
/// </summary>
internal sealed class PsaApi(HttpClient httpClient) : IPsaApi
{
	private readonly HttpClient _httpClient = httpClient;

	public ITicketsApi Tickets => new TicketsApi(_httpClient);
	public IUsersApi Users => new UsersApi(_httpClient);
	public IClientsApi Clients => new ClientsApi(_httpClient);
	public IActionsApi Actions => new ActionsApi(_httpClient);
	public IAttachmentsApi Attachments => new AttachmentsApi(_httpClient);
	public IAssetsApi Assets => new AssetsApi(_httpClient);
	public IProjectsApi Projects => new ProjectsApi(_httpClient);
	public IReportsApi Reports => new ReportsApi(_httpClient);
}

/// <summary>
/// Implementation of ServiceDesk API module
/// </summary>
internal sealed class ServiceDeskApi(HttpClient httpClient) : IServiceDeskApi
{
	private readonly HttpClient _httpClient = httpClient;

	public IKnowledgeBaseApi KnowledgeBase => new KnowledgeBaseApi(_httpClient);
	public IServiceCatalogApi ServiceCatalog => new ServiceCatalogApi(_httpClient);
	public IWorkflowsApi Workflows => new WorkflowsApi(_httpClient);
	public IApprovalsApi Approvals => new ApprovalsApi(_httpClient);
}

/// <summary>
/// Implementation of System API module
/// </summary>
internal sealed class SystemApi(HttpClient httpClient) : ISystemApi
{
	private readonly HttpClient _httpClient = httpClient;

	public IConfigurationApi Configuration => new ConfigurationApi(_httpClient);
	public IIntegrationApi Integration => new IntegrationApi(_httpClient);
	public IAuditApi Audit => new AuditApi(_httpClient);
	public ICustomFieldsApi CustomFields => new CustomFieldsApi(_httpClient);
}

// Placeholder implementations - will be replaced with Refit interfaces in Phase 1.2
#pragma warning disable CS9113 // Parameter is unread
#pragma warning disable IDE0060 // Remove unused parameter
internal sealed class TicketsApi(HttpClient httpClient) : ITicketsApi { }
internal sealed class UsersApi(HttpClient httpClient) : IUsersApi { }
internal sealed class ClientsApi(HttpClient httpClient) : IClientsApi { }
internal sealed class ActionsApi(HttpClient httpClient) : IActionsApi { }
internal sealed class AttachmentsApi(HttpClient httpClient) : IAttachmentsApi { }
internal sealed class AssetsApi(HttpClient httpClient) : IAssetsApi { }
internal sealed class ProjectsApi(HttpClient httpClient) : IProjectsApi { }
internal sealed class ReportsApi(HttpClient httpClient) : IReportsApi { }
internal sealed class KnowledgeBaseApi(HttpClient httpClient) : IKnowledgeBaseApi { }
internal sealed class ServiceCatalogApi(HttpClient httpClient) : IServiceCatalogApi { }
internal sealed class WorkflowsApi(HttpClient httpClient) : IWorkflowsApi { }
internal sealed class ApprovalsApi(HttpClient httpClient) : IApprovalsApi { }
internal sealed class ConfigurationApi(HttpClient httpClient) : IConfigurationApi { }
internal sealed class IntegrationApi(HttpClient httpClient) : IIntegrationApi { }
internal sealed class AuditApi(HttpClient httpClient) : IAuditApi { }
internal sealed class CustomFieldsApi(HttpClient httpClient) : ICustomFieldsApi { }
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CS9113 // Parameter is unread
