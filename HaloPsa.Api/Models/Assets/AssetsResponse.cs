using System.Text.Json.Serialization;

namespace HaloPsa.Api.Models.Assets;

/// <summary>
/// Response wrapper for Assets API - matches Halo API format
/// </summary>
public record AssetsResponse
{
	/// <summary>
	/// The total record count
	/// </summary>
	[JsonPropertyName("record_count")]
	public int RecordCount { get; init; }

	/// <summary>
	/// The list of assets
	/// </summary>
	[JsonPropertyName("assets")]
	public IReadOnlyList<Asset> Assets { get; init; } = [];
}