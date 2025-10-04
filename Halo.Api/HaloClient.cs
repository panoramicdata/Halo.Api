using Halo.Api.Interfaces;

namespace Halo.Api;

/// <summary>
/// Client for interacting with the Halo API
/// </summary>
public class HaloClient(HaloClientOptions options) : IHaloClient
{
	/// <summary>
	/// Gets the Halo account identifier
	/// </summary>
	public string Account { get; } = ValidateAndReturnAccount(options);

	private static string ValidateAndReturnAccount(HaloClientOptions options)
	{
		ArgumentNullException.ThrowIfNull(options);
		options.Validate();
		return options.HaloAccount;
	}

	private static string ValidateAndReturnClientId(HaloClientOptions options)
	{
		ArgumentNullException.ThrowIfNull(options);
		options.Validate();
		return options.HaloClientId;
	}
}
