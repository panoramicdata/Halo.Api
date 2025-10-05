using Halo.Api.Interfaces;
using Halo.Api.Models.Tickets;

namespace Halo.Api.Infrastructure;

/// <summary>
/// Wrapper for the Tickets API that provides convenience methods
/// </summary>
/// <param name="ticketsApi">The underlying Tickets API</param>
public class TicketsApiWrapper(ITicketsApi ticketsApi) : ITicketsApi
{
	/// <inheritdoc />
	public Task<TicketsResponse> GetAllAsync(TicketFilter? filter, CancellationToken cancellationToken)
		=> ticketsApi.GetAllAsync(filter, cancellationToken);

	/// <inheritdoc />
	public Task<TicketsResponse> GetAllAsync(CancellationToken cancellationToken)
		=> ticketsApi.GetAllAsync(cancellationToken);

	/// <inheritdoc />
	public Task<Ticket> GetByIdAsync(int id, bool includeDetails, CancellationToken cancellationToken)
		=> ticketsApi.GetByIdAsync(id, includeDetails, cancellationToken);

	/// <inheritdoc />
	public Task<Ticket> GetByIdAsync(int id, CancellationToken cancellationToken)
		=> ticketsApi.GetByIdAsync(id, cancellationToken);

	/// <inheritdoc />
	public Task<CreateTicketResponse> CreateAsync(CreateTicketRequest request, CancellationToken cancellationToken)
		=> ticketsApi.CreateAsync(request, cancellationToken);

	/// <inheritdoc />
	public Task<UpdateTicketResponse> UpdateAsync(int id, UpdateTicketRequest request, CancellationToken cancellationToken)
		=> ticketsApi.UpdateAsync(id, request, cancellationToken);

	/// <inheritdoc />
	public Task DeleteAsync(int id, CancellationToken cancellationToken)
		=> ticketsApi.DeleteAsync(id, cancellationToken);

	/// <inheritdoc />
	public Task<UpdateTicketResponse> CloseAsync(int id, string resolution, CancellationToken cancellationToken)
		=> ticketsApi.CloseAsync(id, resolution, cancellationToken);

	/// <inheritdoc />
	public Task<UpdateTicketResponse> CloseAsync(int id, CancellationToken cancellationToken)
		=> ticketsApi.CloseAsync(id, cancellationToken);

	/// <inheritdoc />
	public Task<UpdateTicketResponse> ReopenAsync(int id, string reason, CancellationToken cancellationToken)
		=> ticketsApi.ReopenAsync(id, reason, cancellationToken);

	/// <inheritdoc />
	public Task<UpdateTicketResponse> ReopenAsync(int id, CancellationToken cancellationToken)
		=> ticketsApi.ReopenAsync(id, cancellationToken);

	/// <inheritdoc />
	public Task<UpdateTicketResponse> AssignAsync(int id, int agentId, CancellationToken cancellationToken)
		=> ticketsApi.AssignAsync(id, agentId, cancellationToken);

	/// <inheritdoc />
	public Task<UpdateTicketResponse> AssignAsync(int id, int agentId, int teamId, CancellationToken cancellationToken)
		=> ticketsApi.AssignAsync(id, agentId, teamId, cancellationToken);
}