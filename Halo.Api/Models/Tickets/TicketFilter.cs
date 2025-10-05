namespace Halo.Api.Models.Tickets;

/// <summary>
/// Filter options for ticket queries
/// </summary>
public record TicketFilter
{
	/// <summary>
	/// Number of tickets to return (default: 50, max: 1000)
	/// </summary>
	public int? Count { get; init; }

	/// <summary>
	/// Page number for pagination (when using pageinate=true)
	/// </summary>
	public int? PageNo { get; init; }

	/// <summary>
	/// Page size for pagination (when using pageinate=true)
	/// </summary>
	public int? PageSize { get; init; }

	/// <summary>
	/// Whether to use pagination
	/// </summary>
	public bool? Paginate { get; init; }

	/// <summary>
	/// Filter by ticket status IDs (comma-separated or single value)
	/// </summary>
	public string? Status { get; init; }

	/// <summary>
	/// Filter by priority IDs (comma-separated or single value)
	/// </summary>
	public string? Priority { get; init; }

	/// <summary>
	/// Filter by client ID
	/// </summary>
	public int? ClientId { get; init; }

	/// <summary>
	/// Filter by site ID
	/// </summary>
	public int? SiteId { get; init; }

	/// <summary>
	/// Filter by user ID
	/// </summary>
	public int? UserId { get; init; }

	/// <summary>
	/// Filter by assigned agent ID
	/// </summary>
	public int? AgentId { get; init; }

	/// <summary>
	/// Filter by team ID
	/// </summary>
	public int? TeamId { get; init; }

	/// <summary>
	/// Filter by category ID
	/// </summary>
	public int? CategoryId { get; init; }

	/// <summary>
	/// Filter by ticket type ID
	/// </summary>
	public int? TicketTypeId { get; init; }

	/// <summary>
	/// Search string to filter tickets
	/// </summary>
	public string? Search { get; init; }

	/// <summary>
	/// Filter tickets created after this date
	/// </summary>
	public DateTime? StartDate { get; init; }

	/// <summary>
	/// Filter tickets created before this date
	/// </summary>
	public DateTime? EndDate { get; init; }

	/// <summary>
	/// Only return open tickets
	/// </summary>
	public bool? OpenOnly { get; init; }

	/// <summary>
	/// Only return closed tickets
	/// </summary>
	public bool? ClosedOnly { get; init; }

	/// <summary>
	/// Only return tickets assigned to me
	/// </summary>
	public bool? MyTickets { get; init; }

	/// <summary>
	/// Only return unassigned tickets
	/// </summary>
	public bool? UnassignedOnly { get; init; }

	/// <summary>
	/// Include extra details in the response
	/// </summary>
	public bool? IncludeDetails { get; init; }

	/// <summary>
	/// Include child tickets in the response
	/// </summary>
	public bool? IncludeChildren { get; init; }

	/// <summary>
	/// The field to order by (first order)
	/// </summary>
	public string? Order { get; init; }

	/// <summary>
	/// Whether to order descending (first order)
	/// </summary>
	public bool? OrderDesc { get; init; }

	/// <summary>
	/// The field to order by (second order)
	/// </summary>
	public string? Order2 { get; init; }

	/// <summary>
	/// Whether to order descending (second order)
	/// </summary>
	public bool? OrderDesc2 { get; init; }

	/// <summary>
	/// Advanced search criteria (JSON string)
	/// </summary>
	public string? AdvancedSearch { get; init; }

	/// <summary>
	/// Filter by asset ID
	/// </summary>
	public int? AssetId { get; init; }

	/// <summary>
	/// Filter by service ID
	/// </summary>
	public int? ServiceId { get; init; }

	/// <summary>
	/// Filter by supplier ID
	/// </summary>
	public int? SupplierId { get; init; }

	/// <summary>
	/// Filter by contract ID
	/// </summary>
	public int? ContractId { get; init; }

	/// <summary>
	/// Include custom fields in the response (comma-separated field IDs)
	/// </summary>
	public string? IncludeCustomFields { get; init; }

	/// <summary>
	/// Only return tickets that have been updated after this date
	/// </summary>
	public DateTime? LastUpdateAfter { get; init; }

	/// <summary>
	/// Only return tickets that have been updated before this date
	/// </summary>
	public DateTime? LastUpdateBefore { get; init; }
}