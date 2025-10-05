using System.Text.Json.Serialization;

namespace Halo.Api.Models.Sites;

/// <summary>
/// Response wrapper for site list operations
/// </summary>
public record SitesResponse
{
	/// <summary>
	/// The list of sites
	/// </summary>
	[JsonPropertyName("sites")]
	public IReadOnlyList<Site> Sites { get; init; } = [];

	/// <summary>
	/// The total number of sites in the system
	/// </summary>
	[JsonPropertyName("record_count")]
	public int RecordCount { get; init; }
}