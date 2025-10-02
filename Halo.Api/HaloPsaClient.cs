using System.Text.RegularExpressions;

namespace Halo.Api;

public class HaloClient
{
	private static readonly Regex GuidRegex = new(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$", RegexOptions.Compiled);
	private static readonly Regex HaloClientSecretRegex = new(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}-[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$", RegexOptions.Compiled);

	public HaloClient(HaloClientOptions options)
	{
		Validate(options);
	}

	private static void Validate(HaloClientOptions options)
	{
		ArgumentNullException.ThrowIfNull(options);

		if (string.IsNullOrWhiteSpace(options.HaloAccount))
		{
			throw new ArgumentException("HaloAccount cannot be null or empty.", nameof(options.HaloAccount));
		}

		if (string.IsNullOrWhiteSpace(options.HaloClientId))
		{
			throw new ArgumentException("HaloClientId cannot be null or empty.", nameof(options.HaloClientId));
		}

		if (!GuidRegex.IsMatch(options.HaloClientId))
		{
			throw new ArgumentException("HaloClientId must be a valid GUID format (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).", nameof(options.HaloClientId));
		}

		if (string.IsNullOrWhiteSpace(options.HaloClientSecret))
		{
			throw new ArgumentException("HaloClientSecret cannot be null or empty.", nameof(options.HaloClientSecret));
		}

		if (!HaloClientSecretRegex.IsMatch(options.HaloClientSecret))
		{
			throw new ArgumentException("HaloClientSecret must be in the format of two concatenated GUIDs (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).", nameof(options.HaloClientSecret));
		}
	}
}
