namespace HaloPsa.Api.Interfaces;

/// <summary>
/// Main interface for the Halo API client providing access to all API modules
/// </summary>
public interface IHaloClient
{
	/// <summary>
	/// Gets the PSA (Professional Services Automation) API module
	/// </summary>
	IPsaApi Psa { get; }

	/// <summary>
	/// Gets the ServiceDesk API module
	/// </summary>
	IServiceDeskApi ServiceDesk { get; }

	/// <summary>
	/// Gets the System API module for configuration and administration
	/// </summary>
	ISystemApi System { get; }

	/// <summary>
	/// Gets the Halo account identifier
	/// </summary>
	string Account { get; }

	/// <summary>
	/// Gets the base URL for the Halo API
	/// </summary>
	string BaseUrl { get; }
}
