namespace Halo.Api.Interfaces;

/// <summary>
/// Interface for PSA (Professional Services Automation) API operations
/// </summary>
public interface IPsaApi
{
	/// <summary>
	/// Gets the Tickets API for ticket management operations
	/// </summary>
	ITicketsApi Tickets { get; }

	/// <summary>
	/// Gets the TicketTypes API for ticket type management operations
	/// </summary>
	ITicketTypesApi TicketTypes { get; }

	/// <summary>
	/// Gets the Users API for user management operations
	/// </summary>
	IUsersApi Users { get; }

	/// <summary>
	/// Gets the Clients API for client management operations
	/// </summary>
	IClientsApi Clients { get; }

	/// <summary>
	/// Gets the Actions API for ticket action operations
	/// </summary>
	IActionsApi Actions { get; }

	/// <summary>
	/// Gets the Attachments API for file attachment operations
	/// </summary>
	IAttachmentsApi Attachments { get; }

	/// <summary>
	/// Gets the Assets API for asset management operations
	/// </summary>
	IAssetsApi Assets { get; }

	/// <summary>
	/// Gets the Projects API for project management operations
	/// </summary>
	IProjectsApi Projects { get; }

	/// <summary>
	/// Gets the Reports API for reporting and analytics operations
	/// </summary>
	IReportsApi Reports { get; }
}