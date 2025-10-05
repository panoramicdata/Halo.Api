using System.Text.Json.Serialization;

namespace Halo.Api.Models.Priorities;

/// <summary>
/// Response wrapper for priority list operations
/// </summary>
public record PrioritiesResponse
{
	/// <summary>
	/// The list of priorities
	/// </summary>
	[JsonPropertyName("priorities")]
	public IReadOnlyList<Priority> Priorities { get; init; } = [];

	/// <summary>
	/// The total number of priorities in the system
	/// </summary>
	[JsonPropertyName("record_count")]
	public int RecordCount { get; init; }
}