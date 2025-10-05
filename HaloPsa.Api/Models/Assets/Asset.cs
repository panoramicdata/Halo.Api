using System.Text.Json.Serialization;

namespace HaloPsa.Api.Models.Assets;

/// <summary>
/// Represents an asset in the Halo system
/// </summary>
public record Asset
{
	/// <summary>
	/// The unique identifier for the asset
	/// </summary>
	[JsonPropertyName("id")]
	public required int Id { get; init; }

	/// <summary>
	/// The asset inventory number (acts as name/identifier)
	/// </summary>
	[JsonPropertyName("inventory_number")]
	public string Name { get; init; } = string.Empty;

	/// <summary>
	/// Key field for the asset
	/// </summary>
	[JsonPropertyName("key_field")]
	public string? KeyField { get; init; }

	/// <summary>
	/// Second key field for the asset
	/// </summary>
	[JsonPropertyName("key_field2")]
	public string? KeyField2 { get; init; }

	/// <summary>
	/// Third key field for the asset
	/// </summary>
	[JsonPropertyName("key_field3")]
	public string? KeyField3 { get; init; }

	/// <summary>
	/// The client ID this asset belongs to
	/// </summary>
	[JsonPropertyName("client_id")]
	public int? ClientId { get; init; }

	/// <summary>
	/// The client name this asset belongs to
	/// </summary>
	[JsonPropertyName("client_name")]
	public string? ClientName { get; init; }

	/// <summary>
	/// The site where this asset is located
	/// </summary>
	[JsonPropertyName("site_id")]
	public int? SiteId { get; init; }

	/// <summary>
	/// The site name where this asset is located
	/// </summary>
	[JsonPropertyName("site_name")]
	public string? SiteName { get; init; }

	/// <summary>
	/// Business owner ID
	/// </summary>
	[JsonPropertyName("business_owner_id")]
	public int? BusinessOwnerId { get; init; }

	/// <summary>
	/// Business owner name
	/// </summary>
	[JsonPropertyName("business_owner_name")]
	public string? BusinessOwnerName { get; init; }

	/// <summary>
	/// Username associated with the asset
	/// </summary>
	[JsonPropertyName("username")]
	public string? Username { get; init; }

	/// <summary>
	/// Technical owner ID
	/// </summary>
	[JsonPropertyName("technical_owner_id")]
	public int? TechnicalOwnerId { get; init; }

	/// <summary>
	/// Technical owner name
	/// </summary>
	[JsonPropertyName("technical_owner_name")]
	public string? TechnicalOwnerName { get; init; }

	/// <summary>
	/// Asset type ID
	/// </summary>
	[JsonPropertyName("assettype_id")]
	public int? AssetTypeId { get; init; }

	/// <summary>
	/// Asset type name
	/// </summary>
	[JsonPropertyName("assettype_name")]
	public string? AssetType { get; init; }

	/// <summary>
	/// Asset color
	/// </summary>
	[JsonPropertyName("colour")]
	public string? Colour { get; init; }

	/// <summary>
	/// Asset icon
	/// </summary>
	[JsonPropertyName("icon")]
	public string? Icon { get; init; }

	/// <summary>
	/// Whether the asset is inactive
	/// </summary>
	[JsonPropertyName("inactive")]
	public bool IsInactive { get; init; }

	/// <summary>
	/// Contract reference
	/// </summary>
	[JsonPropertyName("contract_ref")]
	public string? ContractRef { get; init; }

	/// <summary>
	/// Supplier ID
	/// </summary>
	[JsonPropertyName("supplier_id")]
	public int? SupplierId { get; init; }

	/// <summary>
	/// Item stock ID
	/// </summary>
	[JsonPropertyName("itemstock_id")]
	public int? ItemStockId { get; init; }

	/// <summary>
	/// Item ID
	/// </summary>
	[JsonPropertyName("item_id")]
	public int? ItemId { get; init; }

	/// <summary>
	/// Item name
	/// </summary>
	[JsonPropertyName("item_name")]
	public string? ItemName { get; init; }

	/// <summary>
	/// Asset criticality
	/// </summary>
	[JsonPropertyName("criticality")]
	public int? Criticality { get; init; }

	/// <summary>
	/// Asset use
	/// </summary>
	[JsonPropertyName("use")]
	public string? Use { get; init; }

	/// <summary>
	/// Device number
	/// </summary>
	[JsonPropertyName("device_number")]
	public int? DeviceNumber { get; init; }

	/// <summary>
	/// Status ID
	/// </summary>
	[JsonPropertyName("status_id")]
	public int? StatusId { get; init; }

	/// <summary>
	/// SLA ID
	/// </summary>
	[JsonPropertyName("sla_id")]
	public int? SlaId { get; init; }

	/// <summary>
	/// Priority ID
	/// </summary>
	[JsonPropertyName("priority_id")]
	public int? PriorityId { get; init; }
}