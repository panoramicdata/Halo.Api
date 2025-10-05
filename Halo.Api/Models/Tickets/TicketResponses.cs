namespace Halo.Api.Models.Tickets;

/// <summary>
/// Response wrapper for ticket collections with pagination support
/// </summary>
public record TicketsResponse
{
	/// <summary>
	/// The list of tickets returned
	/// </summary>
	public required IReadOnlyList<Ticket> Tickets { get; init; } = [];

	/// <summary>
	/// Total number of tickets available (for pagination)
	/// </summary>
	public int RecordCount { get; init; }

	/// <summary>
	/// Current page number (when using pagination)
	/// </summary>
	public int? PageNo { get; init; }

	/// <summary>
	/// Page size used (when using pagination)
	/// </summary>
	public int? PageSize { get; init; }

	/// <summary>
	/// Total number of pages available (when using pagination)
	/// </summary>
	public int? PageCount { get; init; }

	/// <summary>
	/// Whether there are more results available
	/// </summary>
	public bool HasMore { get; init; }

	/// <summary>
	/// Whether pagination was used for this response
	/// </summary>
	public bool IsPaginated { get; init; }
}

/// <summary>
/// Response wrapper for a single ticket operation
/// </summary>
public record TicketResponse
{
	/// <summary>
	/// The ticket data
	/// </summary>
	public required Ticket Ticket { get; init; }

	/// <summary>
	/// Whether the operation was successful
	/// </summary>
	public bool Success { get; init; } = true;

	/// <summary>
	/// Any messages or warnings from the operation
	/// </summary>
	public IReadOnlyList<string>? Messages { get; init; }
}

/// <summary>
/// Response for ticket creation operations
/// </summary>
public record CreateTicketResponse
{
	/// <summary>
	/// The newly created ticket
	/// </summary>
	public required Ticket Ticket { get; init; }

	/// <summary>
	/// Whether the creation was successful
	/// </summary>
	public bool Success { get; init; } = true;

	/// <summary>
	/// Any messages or warnings from the creation
	/// </summary>
	public IReadOnlyList<string>? Messages { get; init; }

	/// <summary>
	/// The ID of the newly created ticket
	/// </summary>
	public int TicketId => Ticket.Id;
}

/// <summary>
/// Response for ticket update operations
/// </summary>
public record UpdateTicketResponse
{
	/// <summary>
	/// The updated ticket
	/// </summary>
	public required Ticket Ticket { get; init; }

	/// <summary>
	/// Whether the update was successful
	/// </summary>
	public bool Success { get; init; } = true;

	/// <summary>
	/// Any messages or warnings from the update
	/// </summary>
	public IReadOnlyList<string>? Messages { get; init; }

	/// <summary>
	/// The ID of the updated ticket
	/// </summary>
	public int TicketId => Ticket.Id;
}