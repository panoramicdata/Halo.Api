using HaloPsa.Api.Interfaces;
using HaloPsa.Api.Models.Assets;

namespace HaloPsa.Api.Infrastructure;

/// <summary>
/// Wrapper for Assets API that provides both raw responses and convenient array access
/// </summary>
public class AssetsApiWrapper(IAssetsRefitApi assetsRefitApi) : IAssetsApi
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

	/// <summary>
	/// Get an asset by ID
	/// </summary>
	public async Task<Asset> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		return await assetsRefitApi.GetByIdAsync(id, cancellationToken);
	}
}