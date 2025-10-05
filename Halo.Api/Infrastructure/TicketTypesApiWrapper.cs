using Halo.Api.Interfaces;
using Halo.Api.Models.TicketTypes;

namespace Halo.Api.Infrastructure;

/// <summary>
/// Simple wrapper for TicketTypes API to match interface pattern
/// </summary>
internal sealed class TicketTypesApiWrapper(ITicketTypesRefitApi ticketTypesRefitApi) : ITicketTypesApi
{
	/// <summary>
	/// Get all ticket types - Returns direct array (no wrapper)
	/// </summary>
	public async Task<IReadOnlyList<TicketType>> GetAllAsync(CancellationToken cancellationToken)
	{
		return await ticketTypesRefitApi.GetAllAsync(cancellationToken);
	}
}