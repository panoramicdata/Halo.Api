using System.Text.Json.Serialization;

namespace HaloPsa.Api.Models.Users;

/// <summary>
/// Response wrapper for Users API - matches Halo API format
/// </summary>
public record UsersResponse
{
	/// <summary>
	/// The total record count
	/// </summary>
	[JsonPropertyName("record_count")]
	public int RecordCount { get; init; }

	/// <summary>
	/// The list of users
	/// </summary>
	[JsonPropertyName("users")]
	public IReadOnlyList<User> Users { get; init; } = [];
}