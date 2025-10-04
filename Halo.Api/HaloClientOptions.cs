using System.Text.RegularExpressions;

namespace Halo.Api;

/// <summary>
/// Configuration options for the Halo API client
/// </summary>
public partial class HaloClientOptions
{
	private static readonly Regex _guidRegex = GetGuidRegex();
	private static readonly Regex _haloClientSecretRegex = GetHaloClientSecretRegex();

	/// <summary>
	/// Gets or sets the Halo account identifier
	/// </summary>
	public required string HaloAccount { get; init; }

	/// <summary>
	/// Gets or sets the Halo client ID (must be in GUID format)
	/// </summary>
	public required string HaloClientId { get; init; }

	/// <summary>
	/// Gets or sets the Halo client secret (must be in the format of two concatenated GUIDs)
	/// </summary>
	public required string HaloClientSecret { get; init; }

	internal void Validate()
	{
		if (string.IsNullOrWhiteSpace(HaloAccount))
		{
			throw new ArgumentException("HaloAccount cannot be null or empty.", nameof(HaloAccount));
		}

		if (string.IsNullOrWhiteSpace(HaloClientId))
		{
			throw new ArgumentException("HaloClientId cannot be null or empty.", nameof(HaloClientId));
		}

		if (string.IsNullOrWhiteSpace(HaloClientSecret))
		{
			throw new ArgumentException("HaloClientSecret cannot be null or empty.", nameof(HaloClientSecret));
		}


		if (!_guidRegex.IsMatch(HaloClientId))
		{
			throw new FormatException("HaloClientId must be a valid GUID format (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
		}

		if (!_haloClientSecretRegex.IsMatch(HaloClientSecret))
		{
			throw new FormatException("HaloClientSecret must be in the format of two concatenated GUIDs (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
		}
	}

	[GeneratedRegex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$", RegexOptions.Compiled)]
	private static partial Regex GetGuidRegex();

	[GeneratedRegex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}-[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$", RegexOptions.Compiled)]
	private static partial Regex GetHaloClientSecretRegex();
}