using System.Text.Json.Serialization;

namespace Halo.Api.Models.Projects;

/// <summary>
/// Represents a project in the Halo system (Projects are implemented as special ticket types in Halo)
/// </summary>
public record Project
{
	/// <summary>
	/// The unique identifier for the project
	/// </summary>
	[JsonPropertyName("id")]
	public required int Id { get; init; }

	/// <summary>
	/// The project summary/name
	/// </summary>
	[JsonPropertyName("summary")]
	public required string Name { get; init; }

	/// <summary>
	/// The project description/details
	/// </summary>
	[JsonPropertyName("details")]
	public string? Description { get; init; }

	/// <summary>
	/// The status ID of the project
	/// </summary>
	[JsonPropertyName("status_id")]
	public int StatusId { get; init; }

	/// <summary>
	/// The ticket type ID
	/// </summary>
	[JsonPropertyName("tickettype_id")]
	public int TicketTypeId { get; init; }

	/// <summary>
	/// The client ID this project belongs to
	/// </summary>
	[JsonPropertyName("client_id")]
	public int? ClientId { get; init; }

	/// <summary>
	/// The client name this project belongs to
	/// </summary>
	[JsonPropertyName("client_name")]
	public string? ClientName { get; init; }

	/// <summary>
	/// The site ID where this project is located
	/// </summary>
	[JsonPropertyName("site_id")]
	public int? SiteId { get; init; }

	/// <summary>
	/// The site name where this project is located
	/// </summary>
	[JsonPropertyName("site_name")]
	public string? SiteName { get; init; }

	/// <summary>
	/// The assigned user ID (project manager)
	/// </summary>
	[JsonPropertyName("user_id")]
	public int? UserId { get; init; }

	/// <summary>
	/// The assigned user name (project manager)
	/// </summary>
	[JsonPropertyName("user_name")]
	public string? UserName { get; init; }

	/// <summary>
	/// The assigned agent ID
	/// </summary>
	[JsonPropertyName("agent_id")]
	public int? AgentId { get; init; }

	/// <summary>
	/// The project start date
	/// </summary>
	[JsonPropertyName("startdate")]
	public string? StartDate { get; init; }

	/// <summary>
	/// The project target date
	/// </summary>
	[JsonPropertyName("targetdate")]
	public string? TargetDate { get; init; }

	/// <summary>
	/// The project deadline date
	/// </summary>
	[JsonPropertyName("deadlinedate")]
	public string? DeadlineDate { get; init; }

	/// <summary>
	/// Whether the project is inactive
	/// </summary>
	[JsonPropertyName("inactive")]
	public bool IsInactive { get; init; }

	/// <summary>
	/// The project priority ID
	/// </summary>
	[JsonPropertyName("priority_id")]
	public int? PriorityId { get; init; }

	/// <summary>
	/// The estimated time for the project
	/// </summary>
	[JsonPropertyName("estimate")]
	public decimal? Estimate { get; init; }

	/// <summary>
	/// The estimated days for the project
	/// </summary>
	[JsonPropertyName("estimatedays")]
	public decimal? EstimateDays { get; init; }

	/// <summary>
	/// The actual project time
	/// </summary>
	[JsonPropertyName("projecttimeactual")]
	public decimal? ProjectTimeActual { get; init; }

	/// <summary>
	/// The actual project cost
	/// </summary>
	[JsonPropertyName("projectmoneyactual")]
	public decimal? ProjectMoneyActual { get; init; }

	/// <summary>
	/// The project cost
	/// </summary>
	[JsonPropertyName("cost")]
	public decimal? Cost { get; init; }

	/// <summary>
	/// Project date occurred
	/// </summary>
	[JsonPropertyName("dateoccurred")]
	public string? DateOccurred { get; init; }

	/// <summary>
	/// Last action date
	/// </summary>
	[JsonPropertyName("lastactiondate")]
	public string? LastActionDate { get; init; }

	/// <summary>
	/// Last update date
	/// </summary>
	[JsonPropertyName("last_update")]
	public string? LastUpdate { get; init; }
}