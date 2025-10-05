using System.Text.Json.Serialization;

namespace Halo.Api.Models.TicketTypes;

/// <summary>
/// Represents a ticket type in the Halo system
/// </summary>
public record TicketType
{
	/// <summary>
	/// The unique identifier for the ticket type
	/// </summary>
	[JsonPropertyName("id")]
	public required int Id { get; init; }

	/// <summary>
	/// The name of the ticket type
	/// </summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>
	/// Whether the ticket type is inactive
	/// </summary>
	[JsonPropertyName("inactive")]
	public bool IsInactive { get; init; }

	/// <summary>
	/// The colour associated with this ticket type
	/// </summary>
	[JsonPropertyName("colour")]
	public string? Colour { get; init; }

	/// <summary>
	/// Whether this ticket type is default
	/// </summary>
	[JsonPropertyName("isdefault")]
	public bool IsDefault { get; init; }

	/// <summary>
	/// The description of the ticket type
	/// </summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }
}