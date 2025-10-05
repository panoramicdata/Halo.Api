using Halo.Api.Interfaces;
using Halo.Api.Models.Users;

namespace Halo.Api.Infrastructure;

/// <summary>
/// Wrapper for Users API that provides both raw responses and convenient array access
/// </summary>
internal sealed class UsersApiWrapper(IUsersRefitApi usersRefitApi) : IUsersApi
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
}