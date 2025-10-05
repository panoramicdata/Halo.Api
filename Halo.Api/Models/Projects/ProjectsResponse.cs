using System.Text.Json.Serialization;

namespace Halo.Api.Models.Projects;

/// <summary>
/// Response wrapper for Projects API - matches Halo API format
/// Note: Projects API returns "tickets" array, not "projects"
/// </summary>
public record ProjectsResponse
{
	/// <summary>
	/// The total record count
	/// </summary>
	[JsonPropertyName("record_count")]
	public int RecordCount { get; init; }

	/// <summary>
	/// The list of projects (returned as "tickets" by the API)
	/// </summary>
	[JsonPropertyName("tickets")]
	public IReadOnlyList<Project> Projects { get; init; } = [];

	/// <summary>
	/// Whether to include children
	/// </summary>
	[JsonPropertyName("include_children")]
	public bool IncludeChildren { get; init; }
}