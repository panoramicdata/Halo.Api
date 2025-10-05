using System.Text.Json.Serialization;

namespace HaloPsa.Api.Models.Statuses;

/// <summary>
/// Response wrapper for status list operations
/// </summary>
public record StatusesResponse
{
	/// <summary>
	/// The list of statuses
	/// </summary>
	[JsonPropertyName("statuses")]
	public IReadOnlyList<Status> Statuses { get; init; } = [];

	/// <summary>
	/// The total number of statuses in the system
	/// </summary>
	[JsonPropertyName("record_count")]
	public int RecordCount { get; init; }
}