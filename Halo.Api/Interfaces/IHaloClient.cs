namespace Halo.Api.Interfaces;

/// <summary>
/// Interface for Halo API client operations
/// </summary>
public interface IHaloClient
{
	/// <summary>
	/// Gets the Halo account identifier
	/// </summary>
	string Account { get; }
}
