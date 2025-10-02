
using System.Text.RegularExpressions;

namespace Halo.Api;

public partial class HaloClientOptions
{
	private static readonly Regex _guidRegex = GetGuidRegex();
	private static readonly Regex _haloClientSecretRegex = GetHaloClientSecretRegex();

	public required string HaloAccount { get; init; }

	public required string HaloClientId { get; init; }

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