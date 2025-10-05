using Halo.Api.Interfaces;
using Halo.Api.Models.Categories;

namespace Halo.Api.Infrastructure;

/// <summary>
/// Wrapper for the Categories API that provides convenience methods
/// </summary>
/// <param name="categoriesApi">The underlying Categories API</param>
public class CategoriesApiWrapper(ICategoriesApi categoriesApi) : ICategoriesApi
{
	/// <summary>
	/// Gets all categories as a convenient list
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of categories</returns>
	public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken)
	{
		var response = await categoriesApi.GetAllResponseAsync(ticketTypeId: null, teamId: null, cancellationToken);
		return response.Categories;
	}

	/// <summary>
	/// Gets all categories filtered by ticket type
	/// </summary>
	/// <param name="ticketTypeId">The ticket type ID to filter by</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of categories for the specified ticket type</returns>
	public async Task<IReadOnlyList<Category>> GetAllAsync(int ticketTypeId, CancellationToken cancellationToken)
	{
		var response = await categoriesApi.GetAllResponseAsync(ticketTypeId, teamId: null, cancellationToken);
		return response.Categories;
	}

	/// <inheritdoc />
	public Task<CategoriesResponse> GetAllResponseAsync(int? ticketTypeId, int? teamId, CancellationToken cancellationToken)
		=> categoriesApi.GetAllResponseAsync(ticketTypeId, teamId, cancellationToken);

	/// <inheritdoc />
	public Task<Category> GetByIdAsync(int id, bool includeDetails, CancellationToken cancellationToken)
		=> categoriesApi.GetByIdAsync(id, includeDetails, cancellationToken);

	/// <summary>
	/// Gets a specific category by ID with default parameters
	/// </summary>
	/// <param name="id">The category ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The category with the specified ID</returns>
	public Task<Category> GetByIdAsync(int id, CancellationToken cancellationToken)
		=> GetByIdAsync(id, includeDetails: false, cancellationToken);
}