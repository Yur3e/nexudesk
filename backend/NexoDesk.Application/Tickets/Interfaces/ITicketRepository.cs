using NexoDesk.Domain.Entities;
using NexoDesk.Application.Tickets.Models;

namespace NexoDesk.Application.Tickets.Interfaces;

public interface ITicketRepository
{
    Task AddAsync(Ticket ticket, CancellationToken cancellationToken);

    Task<TicketPage> GetPageAsync(
        Guid? createdByUserId,
        TicketFilter filter,
        CancellationToken cancellationToken);

    Task<Ticket?> GetByIdAsync(Guid ticketId, CancellationToken cancellationToken);

    Task<Ticket?> GetByIdForUpdateAsync(Guid ticketId, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
