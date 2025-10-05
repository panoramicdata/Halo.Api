using System.Text.Json.Serialization;

namespace Halo.Api.Models.Assets;

/// <summary>
/// Request model for creating a new asset
/// </summary>
public record CreateAssetRequest
{
	/// <summary>
	/// The asset name/tag (required)
	/// </summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>
	/// The asset type
	/// </summary>
	[JsonPropertyName("assettype")]
	public string? AssetType { get; init; }

	/// <summary>
	/// The asset's serial number
	/// </summary>
	[JsonPropertyName("serial")]
	public string? Serial { get; init; }

	/// <summary>
	/// The asset's inventory number
	/// </summary>
	[JsonPropertyName("inventory_number")]
	public string? InventoryNumber { get; init; }

	/// <summary>
	/// The client ID this asset belongs to
	/// </summary>
	[JsonPropertyName("client_id")]
	public int? ClientId { get; init; }

	/// <summary>
	/// The site where this asset is located
	/// </summary>
	[JsonPropertyName("site_id")]
	public int? SiteId { get; init; }

	/// <summary>
	/// The assigned user ID
	/// </summary>
	[JsonPropertyName("assignedto_id")]
	public int? AssignedToId { get; init; }

	/// <summary>
	/// The asset make/manufacturer
	/// </summary>
	[JsonPropertyName("make")]
	public string? Make { get; init; }

	/// <summary>
	/// The asset model
	/// </summary>
	[JsonPropertyName("model")]
	public string? Model { get; init; }

	/// <summary>
	/// The asset supplier
	/// </summary>
	[JsonPropertyName("supplier")]
	public string? Supplier { get; init; }

	/// <summary>
	/// The asset warranty expiry date
	/// </summary>
	[JsonPropertyName("warranty")]
	public string? Warranty { get; init; }

	/// <summary>
	/// The asset location
	/// </summary>
	[JsonPropertyName("location")]
	public string? Location { get; init; }

	/// <summary>
	/// Additional notes about the asset
	/// </summary>
	[JsonPropertyName("notes")]
	public string? Notes { get; init; }

	/// <summary>
	/// Whether the asset is inactive
	/// </summary>
	[JsonPropertyName("inactive")]
	public bool IsInactive { get; init; }
}

/// <summary>
/// Request model for updating an existing asset
/// </summary>
public record UpdateAssetRequest
{
	/// <summary>
	/// The asset name/tag
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }

	/// <summary>
	/// The asset type
	/// </summary>
	[JsonPropertyName("assettype")]
	public string? AssetType { get; init; }

	/// <summary>
	/// The asset's serial number
	/// </summary>
	[JsonPropertyName("serial")]
	public string? Serial { get; init; }

	/// <summary>
	/// The asset's inventory number
	/// </summary>
	[JsonPropertyName("inventory_number")]
	public string? InventoryNumber { get; init; }

	/// <summary>
	/// The client ID this asset belongs to
	/// </summary>
	[JsonPropertyName("client_id")]
	public int? ClientId { get; init; }

	/// <summary>
	/// The site where this asset is located
	/// </summary>
	[JsonPropertyName("site_id")]
	public int? SiteId { get; init; }

	/// <summary>
	/// The assigned user ID
	/// </summary>
	[JsonPropertyName("assignedto_id")]
	public int? AssignedToId { get; init; }

	/// <summary>
	/// The asset make/manufacturer
	/// </summary>
	[JsonPropertyName("make")]
	public string? Make { get; init; }

	/// <summary>
	/// The asset model
	/// </summary>
	[JsonPropertyName("model")]
	public string? Model { get; init; }

	/// <summary>
	/// The asset supplier
	/// </summary>
	[JsonPropertyName("supplier")]
	public string? Supplier { get; init; }

	/// <summary>
	/// The asset warranty expiry date
	/// </summary>
	[JsonPropertyName("warranty")]
	public string? Warranty { get; init; }

	/// <summary>
	/// The asset location
	/// </summary>
	[JsonPropertyName("location")]
	public string? Location { get; init; }

	/// <summary>
	/// Additional notes about the asset
	/// </summary>
	[JsonPropertyName("notes")]
	public string? Notes { get; init; }

	/// <summary>
	/// Whether the asset is inactive
	/// </summary>
	[JsonPropertyName("inactive")]
	public bool? IsInactive { get; init; }
}

/// <summary>
/// Response for asset creation operations
/// </summary>
public record CreateAssetResponse
{
	/// <summary>
	/// The newly created asset
	/// </summary>
	public required Asset Asset { get; init; }

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
/// Response for asset update operations
/// </summary>
public record UpdateAssetResponse
{
	/// <summary>
	/// The updated asset
	/// </summary>
	public required Asset Asset { get; init; }

	/// <summary>
	/// Whether the update was successful
	/// </summary>
	public bool Success { get; init; } = true;

	/// <summary>
	/// Any messages or warnings from the update
	/// </summary>
	public IReadOnlyList<string>? Messages { get; init; }
}