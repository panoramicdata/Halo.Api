using Halo.Api.Interfaces;
using Halo.Api.Models.Users;

namespace Halo.Api.Infrastructure;

/// <summary>
/// Wrapper for Users API that provides both raw responses and convenient array access
/// </summary>
public class UsersApiWrapper(IUsersRefitApi usersRefitApi) : IUsersApi
{
	/// <summary>
	/// Get all users - Returns unwrapped array for convenience
	/// </summary>
	public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
	{
		var response = await GetResponseAsync(cancellationToken);
		return response.Users;
	}

	/// <summary>
	/// Get all users - Returns wrapped response
	/// </summary>
	public async Task<UsersResponse> GetResponseAsync(CancellationToken cancellationToken)
	{
		return await usersRefitApi.GetResponseAsync(cancellationToken);
	}

	/// <summary>
	/// Get a user by ID
	/// </summary>
	public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		return await usersRefitApi.GetByIdAsync(id, cancellationToken);
	}
}