using Halo.Api.Interfaces;
using Halo.Api.Models.Statuses;

namespace Halo.Api.Infrastructure;

/// <summary>
/// Wrapper for the Statuses API that provides convenience methods
/// </summary>
/// <param name="statusesApi">The underlying Statuses API</param>
public class StatusesApiWrapper(IStatusesApi statusesApi) : IStatusesApi
{
	/// <summary>
	/// Gets all statuses as a convenient list
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of statuses</returns>
	public async Task<IReadOnlyList<Status>> GetAllAsync(CancellationToken cancellationToken)
	{
		var response = await statusesApi.GetAllResponseAsync(cancellationToken);
		return response.Statuses;
	}

	/// <inheritdoc />
	public Task<StatusesResponse> GetAllResponseAsync(CancellationToken cancellationToken)
		=> statusesApi.GetAllResponseAsync(cancellationToken);

	/// <inheritdoc />
	public Task<Status> GetByIdAsync(int id, bool includeDetails, CancellationToken cancellationToken)
		=> statusesApi.GetByIdAsync(id, includeDetails, cancellationToken);

	/// <summary>
	/// Gets a specific status by ID with default parameters
	/// </summary>
	/// <param name="id">The status ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The status with the specified ID</returns>
	public Task<Status> GetByIdAsync(int id, CancellationToken cancellationToken)
		=> GetByIdAsync(id, includeDetails: false, cancellationToken);
}