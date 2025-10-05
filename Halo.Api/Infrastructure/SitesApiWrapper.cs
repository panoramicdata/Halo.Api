using Halo.Api.Interfaces;
using Halo.Api.Models.Sites;

namespace Halo.Api.Infrastructure;

/// <summary>
/// Wrapper for the Sites API that provides convenience methods
/// </summary>
/// <param name="sitesApi">The underlying Sites API</param>
public class SitesApiWrapper(ISitesApi sitesApi) : ISitesApi
{
	/// <summary>
	/// Gets all sites as a convenient list
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of sites</returns>
	public async Task<IReadOnlyList<Site>> GetAllAsync(CancellationToken cancellationToken)
	{
		var response = await sitesApi.GetAllResponseAsync(clientId: null, includeInactive: false, cancellationToken);
		return response.Sites;
	}

	/// <summary>
	/// Gets all sites for a specific client
	/// </summary>
	/// <param name="clientId">The client ID to filter by</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of sites for the specified client</returns>
	public async Task<IReadOnlyList<Site>> GetAllAsync(int clientId, CancellationToken cancellationToken)
	{
		var response = await sitesApi.GetAllResponseAsync(clientId, includeInactive: false, cancellationToken);
		return response.Sites;
	}

	/// <inheritdoc />
	public Task<SitesResponse> GetAllResponseAsync(int? clientId, bool includeInactive, CancellationToken cancellationToken)
		=> sitesApi.GetAllResponseAsync(clientId, includeInactive, cancellationToken);

	/// <inheritdoc />
	public Task<Site> GetByIdAsync(int id, bool includeDetails, CancellationToken cancellationToken)
		=> sitesApi.GetByIdAsync(id, includeDetails, cancellationToken);

	/// <summary>
	/// Gets a specific site by ID with default parameters
	/// </summary>
	/// <param name="id">The site ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The site with the specified ID</returns>
	public Task<Site> GetByIdAsync(int id, CancellationToken cancellationToken)
		=> GetByIdAsync(id, includeDetails: false, cancellationToken);
}