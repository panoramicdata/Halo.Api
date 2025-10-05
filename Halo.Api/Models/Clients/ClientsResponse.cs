using System.Text.Json.Serialization;

namespace Halo.Api.Models.Clients;

/// <summary>
/// Response wrapper for Clients API - matches Halo API format
/// </summary>
public record ClientsResponse
{
	/// <summary>
	/// The total record count
	/// </summary>
	[JsonPropertyName("record_count")]
	public int RecordCount { get; init; }

	/// <summary>
	/// The list of clients
	/// </summary>
	[JsonPropertyName("clients")]
	public IReadOnlyList<Client> Clients { get; init; } = [];
}