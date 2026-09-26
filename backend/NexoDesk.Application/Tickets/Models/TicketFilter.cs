using NexoDesk.Domain.Enums;

namespace NexoDesk.Application.Tickets.Models;

public sealed record TicketFilter(
    TicketStatus? Status,
    TicketPriority? Priority,
    bool? Assigned,
    int Page,
    int PageSize);
