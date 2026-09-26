using NexoDesk.Application.Tickets.Interfaces;
using NexoDesk.Application.Tickets.Models;
using NexoDesk.Domain.Entities;
using NexoDesk.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace NexoDesk.Infrastructure.Repositories;

public sealed class TicketRepository(HelpDeskDbContext context) : ITicketRepository
{
    public async Task AddAsync(Ticket ticket, CancellationToken cancellationToken)
    {
        await context.Tickets.AddAsync(ticket, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<TicketPage> GetPageAsync(
        Guid? createdByUserId,
        TicketFilter filter,
        CancellationToken cancellationToken)
    {
        var query = context.Tickets
            .AsNoTracking()
            .Include(ticket => ticket.AssignedAgent)
            .AsQueryable();

        if (createdByUserId.HasValue)
        {
            query = query.Where(ticket => ticket.CreatedByUserId == createdByUserId.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(ticket => ticket.Status == filter.Status.Value);
        }

        if (filter.Priority.HasValue)
        {
            query = query.Where(ticket => ticket.Priority == filter.Priority.Value);
        }

        if (filter.Assigned.HasValue)
        {
            query = filter.Assigned.Value
                ? query.Where(ticket => ticket.AssignedAgentId != null)
                : query.Where(ticket => ticket.AssignedAgentId == null);
        }

        var totalItems = await query.CountAsync(cancellationToken);
        var tickets = await query
            .OrderByDescending(ticket => ticket.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new TicketPage(tickets, totalItems);
    }

    public Task<Ticket?> GetByIdAsync(Guid ticketId, CancellationToken cancellationToken) =>
        context.Tickets
            .AsNoTracking()
            .Include(ticket => ticket.CreatedByUser)
            .Include(ticket => ticket.AssignedAgent)
            .SingleOrDefaultAsync(ticket => ticket.Id == ticketId, cancellationToken);

    public Task<Ticket?> GetByIdForUpdateAsync(Guid ticketId, CancellationToken cancellationToken) =>
        context.Tickets
            .Include(ticket => ticket.CreatedByUser)
            .Include(ticket => ticket.AssignedAgent)
            .SingleOrDefaultAsync(ticket => ticket.Id == ticketId, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        context.SaveChangesAsync(cancellationToken);
}
