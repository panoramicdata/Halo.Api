using Halo.Api.Interfaces;
using Halo.Api.Models.Projects;

namespace Halo.Api.Infrastructure;

/// <summary>
/// Wrapper for Projects API that provides both raw responses and convenient array access
/// </summary>
internal sealed class ProjectsApiWrapper(IProjectsRefitApi projectsRefitApi) : IProjectsApi
{
	/// <summary>
	/// Get all projects - Returns unwrapped array for convenience
	/// </summary>
	public async Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken)
	{
		var response = await GetResponseAsync(cancellationToken);
		return response.Projects;
	}

	/// <summary>
	/// Get all projects - Returns wrapped response
	/// </summary>
	public async Task<ProjectsResponse> GetResponseAsync(CancellationToken cancellationToken)
	{
		return await projectsRefitApi.GetResponseAsync(cancellationToken);
	}
}