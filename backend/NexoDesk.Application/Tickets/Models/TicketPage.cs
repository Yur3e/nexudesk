using NexoDesk.Domain.Entities;

namespace NexoDesk.Application.Tickets.Models;

public sealed record TicketPage(IReadOnlyList<Ticket> Items, int TotalItems);
