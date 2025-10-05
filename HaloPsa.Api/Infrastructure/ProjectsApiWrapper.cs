using HaloPsa.Api.Interfaces;
using HaloPsa.Api.Models.Projects;

namespace HaloPsa.Api.Infrastructure;

/// <summary>
/// Wrapper for Projects API that provides both raw responses and convenient array access
/// </summary>
public class ProjectsApiWrapper(IProjectsRefitApi projectsRefitApi) : IProjectsApi
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

	/// <summary>
	/// Get a project by ID
	/// </summary>
	public async Task<Project> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		return await projectsRefitApi.GetByIdAsync(id, cancellationToken);
	}
}