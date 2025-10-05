using System.Text.Json.Serialization;

namespace Halo.Api.Models.Users;

/// <summary>
/// Request model for creating a new user
/// </summary>
public record CreateUserRequest
{
	/// <summary>
	/// The user's name (required)
	/// </summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>
	/// The user's email address
	/// </summary>
	[JsonPropertyName("emailaddress")]
	public string? EmailAddress { get; init; }

	/// <summary>
	/// The user's username/login
	/// </summary>
	[JsonPropertyName("username")]
	public string? Username { get; init; }

	/// <summary>
	/// The user's site/location
	/// </summary>
	[JsonPropertyName("site")]
	public string? Site { get; init; }

	/// <summary>
	/// The user's department
	/// </summary>
	[JsonPropertyName("department")]
	public string? Department { get; init; }

	/// <summary>
	/// The user's telephone number
	/// </summary>
	[JsonPropertyName("telephone")]
	public string? Telephone { get; init; }

	/// <summary>
	/// The user's mobile number
	/// </summary>
	[JsonPropertyName("mobile")]
	public string? Mobile { get; init; }

	/// <summary>
	/// Whether this user is an agent
	/// </summary>
	[JsonPropertyName("isagent")]
	public bool IsAgent { get; init; }

	/// <summary>
	/// Whether this user is an important user
	/// </summary>
	[JsonPropertyName("isimportantuser")]
	public bool IsImportantUser { get; init; }

	/// <summary>
	/// The client ID this user belongs to
	/// </summary>
	[JsonPropertyName("client_id")]
	public int? ClientId { get; init; }

	/// <summary>
	/// Whether the user is inactive
	/// </summary>
	[JsonPropertyName("inactive")]
	public bool IsInactive { get; init; }
}

/// <summary>
/// Request model for updating an existing user
/// </summary>
public record UpdateUserRequest
{
	/// <summary>
	/// The user's name
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }

	/// <summary>
	/// The user's email address
	/// </summary>
	[JsonPropertyName("emailaddress")]
	public string? EmailAddress { get; init; }

	/// <summary>
	/// The user's username/login
	/// </summary>
	[JsonPropertyName("username")]
	public string? Username { get; init; }

	/// <summary>
	/// The user's site/location
	/// </summary>
	[JsonPropertyName("site")]
	public string? Site { get; init; }

	/// <summary>
	/// The user's department
	/// </summary>
	[JsonPropertyName("department")]
	public string? Department { get; init; }

	/// <summary>
	/// The user's telephone number
	/// </summary>
	[JsonPropertyName("telephone")]
	public string? Telephone { get; init; }

	/// <summary>
	/// The user's mobile number
	/// </summary>
	[JsonPropertyName("mobile")]
	public string? Mobile { get; init; }

	/// <summary>
	/// Whether this user is an agent
	/// </summary>
	[JsonPropertyName("isagent")]
	public bool? IsAgent { get; init; }

	/// <summary>
	/// Whether this user is an important user
	/// </summary>
	[JsonPropertyName("isimportantuser")]
	public bool? IsImportantUser { get; init; }

	/// <summary>
	/// The client ID this user belongs to
	/// </summary>
	[JsonPropertyName("client_id")]
	public int? ClientId { get; init; }

	/// <summary>
	/// Whether the user is inactive
	/// </summary>
	[JsonPropertyName("inactive")]
	public bool? IsInactive { get; init; }
}

/// <summary>
/// Response for user creation operations
/// </summary>
public record CreateUserResponse
{
	/// <summary>
	/// The newly created user
	/// </summary>
	public required User User { get; init; }

	/// <summary>
	/// Whether the creation was successful
	/// </summary>
	public bool Success { get; init; } = true;

	/// <summary>
	/// Any messages or warnings from the creation
	/// </summary>
	public IReadOnlyList<string>? Messages { get; init; }
}

/// <summary>
/// Response for user update operations
/// </summary>
public record UpdateUserResponse
{
	/// <summary>
	/// The updated user
	/// </summary>
	public required User User { get; init; }

	/// <summary>
	/// Whether the update was successful
	/// </summary>
	public bool Success { get; init; } = true;

	/// <summary>
	/// Any messages or warnings from the update
	/// </summary>
	public IReadOnlyList<string>? Messages { get; init; }
}