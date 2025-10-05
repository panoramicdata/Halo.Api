using HaloPsa.Api.Interfaces;
using HaloPsa.Api.Models.Clients;

namespace HaloPsa.Api.Infrastructure;

/// <summary>
/// Wrapper for Clients API that provides both raw responses and convenient array access
/// </summary>
public class ClientsApiWrapper(IClientsRefitApi clientsRefitApi) : IClientsApi
{
	/// <summary>
	/// Get all clients - Returns unwrapped array for convenience
	/// </summary>
	public async Task<IReadOnlyList<Client>> GetAllAsync(CancellationToken cancellationToken)
	{
		var response = await GetResponseAsync(cancellationToken);
		return response.Clients;
	}

	/// <summary>
	/// Get all clients - Returns wrapped response
	/// </summary>
	public async Task<ClientsResponse> GetResponseAsync(CancellationToken cancellationToken)
	{
		return await clientsRefitApi.GetResponseAsync(cancellationToken);
	}

	/// <summary>
	/// Get a client by ID
	/// </summary>
	public async Task<Client> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		return await clientsRefitApi.GetByIdAsync(id, cancellationToken);
	}
}