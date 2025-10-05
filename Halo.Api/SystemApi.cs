using Halo.Api.Interfaces;

namespace Halo.Api;

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
#pragma warning restore CS9113 // Parameter is unread
