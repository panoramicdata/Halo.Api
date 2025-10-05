using HaloPsa.Api.Interfaces;
using HaloPsa.Api.Models.TicketTypes;

namespace HaloPsa.Api.Infrastructure;

/// <summary>
/// Simple wrapper for TicketTypes API to match interface pattern
/// </summary>
public class TicketTypesApiWrapper(ITicketTypesRefitApi ticketTypesRefitApi) : ITicketTypesApi
{
	/// <summary>
	/// Get all ticket types - Returns direct array (no wrapper)
	/// </summary>
	public async Task<IReadOnlyList<TicketType>> GetAllAsync(CancellationToken cancellationToken)
	{
		return await ticketTypesRefitApi.GetAllAsync(cancellationToken);
	}

	/// <summary>
	/// Get a ticket type by ID
	/// </summary>
	public async Task<TicketType> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		return await ticketTypesRefitApi.GetByIdAsync(id, cancellationToken);
	}
}