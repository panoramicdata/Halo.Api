using Halo.Api.Interfaces;
using Halo.Api.Models.Teams;

namespace Halo.Api.Infrastructure;

/// <summary>
/// Wrapper for the Teams API that provides convenience methods
/// </summary>
/// <param name="teamsApi">The underlying Teams API</param>
public class TeamsApiWrapper(ITeamsApi teamsApi) : ITeamsApi
{
	/// <summary>
	/// Gets all teams as a convenient list
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of teams</returns>
	public async Task<IReadOnlyList<Team>> GetAllAsync(CancellationToken cancellationToken)
	{
		var response = await teamsApi.GetAllResponseAsync(cancellationToken);
		return response.Teams;
	}

	/// <inheritdoc />
	public Task<TeamsResponse> GetAllResponseAsync(CancellationToken cancellationToken)
		=> teamsApi.GetAllResponseAsync(cancellationToken);

	/// <inheritdoc />
	public Task<Team> GetByIdAsync(int id, bool includeDetails, CancellationToken cancellationToken)
		=> teamsApi.GetByIdAsync(id, includeDetails, cancellationToken);

	/// <summary>
	/// Gets a specific team by ID with default parameters
	/// </summary>
	/// <param name="id">The team ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The team with the specified ID</returns>
	public Task<Team> GetByIdAsync(int id, CancellationToken cancellationToken)
		=> GetByIdAsync(id, includeDetails: false, cancellationToken);
}