using Halo.Api.Interfaces;

namespace Halo.Api;

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
#pragma warning restore CS9113 // Parameter is unread
