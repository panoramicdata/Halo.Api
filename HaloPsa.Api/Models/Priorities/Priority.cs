using System.Text.Json.Serialization;

namespace HaloPsa.Api.Models.Priorities;

/// <summary>
/// Represents a ticket priority in the Halo system
/// </summary>
public record Priority
{
	/// <summary>
	/// The unique identifier for the priority
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; init; }

	/// <summary>
	/// The name of the priority
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; init; } = string.Empty;

	/// <summary>
	/// The description of the priority
	/// </summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>
	/// The color associated with this priority
	/// </summary>
	[JsonPropertyName("colour")]
	public string? Color { get; init; }

	/// <summary>
	/// The numerical priority value (lower = higher priority)
	/// </summary>
	[JsonPropertyName("priorityvalue")]
	public int PriorityValue { get; init; }

	/// <summary>
	/// Whether this priority is active
	/// </summary>
	[JsonPropertyName("inactive")]
	public bool IsInactive { get; init; }

	/// <summary>
	/// Whether this is the default priority
	/// </summary>
	[JsonPropertyName("isdefault")]
	public bool IsDefault { get; init; }

	/// <summary>
	/// The order in which this priority appears in lists
	/// </summary>
	[JsonPropertyName("order")]
	public int Order { get; init; }
}