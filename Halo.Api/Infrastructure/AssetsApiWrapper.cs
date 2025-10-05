using Halo.Api.Interfaces;
using Halo.Api.Models.Assets;

namespace Halo.Api.Infrastructure;

/// <summary>
/// Wrapper for Assets API that provides both raw responses and convenient array access
/// </summary>
internal sealed class AssetsApiWrapper(IAssetsRefitApi assetsRefitApi) : IAssetsApi
{
	/// <summary>
	/// Get all assets - Returns unwrapped array for convenience
	/// </summary>
	public async Task<IReadOnlyList<Asset>> GetAllAsync(CancellationToken cancellationToken)
	{
		var response = await GetResponseAsync(cancellationToken);
		return response.Assets;
	}

	/// <summary>
	/// Get all assets - Returns wrapped response
	/// </summary>
	public async Task<AssetsResponse> GetResponseAsync(CancellationToken cancellationToken)
	{
		return await assetsRefitApi.GetResponseAsync(cancellationToken);
	}
}