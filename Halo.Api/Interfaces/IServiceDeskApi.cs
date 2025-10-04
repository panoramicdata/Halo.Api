namespace Halo.Api.Interfaces;

/// <summary>
/// Interface for ServiceDesk API operations
/// </summary>
public interface IServiceDeskApi
{
	/// <summary>
	/// Gets the Knowledge Base API for article and knowledge management operations
	/// </summary>
	IKnowledgeBaseApi KnowledgeBase { get; }

	/// <summary>
	/// Gets the Service Catalog API for service request operations
	/// </summary>
	IServiceCatalogApi ServiceCatalog { get; }

	/// <summary>
	/// Gets the Workflows API for workflow and automation operations
	/// </summary>
	IWorkflowsApi Workflows { get; }

	/// <summary>
	/// Gets the Approvals API for approval process operations
	/// </summary>
	IApprovalsApi Approvals { get; }
}