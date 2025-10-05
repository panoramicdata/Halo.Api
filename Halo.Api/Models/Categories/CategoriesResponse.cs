using System.Text.Json.Serialization;

namespace Halo.Api.Models.Categories;

/// <summary>
/// Response wrapper for category list operations
/// </summary>
public record CategoriesResponse
{
	/// <summary>
	/// The list of categories
	/// </summary>
	[JsonPropertyName("categories")]
	public IReadOnlyList<Category> Categories { get; init; } = [];

	/// <summary>
	/// The total number of categories in the system
	/// </summary>
	[JsonPropertyName("record_count")]
	public int RecordCount { get; init; }
}