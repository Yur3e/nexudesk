namespace NexoDesk.Application.Tickets.Contracts;

public sealed record TicketResponse(
    Guid Id,
    string Title,
    string Description,
    string Status,
    string Priority,
    Guid CreatedByUserId,
    Guid? AssignedAgentId,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? ClosedAt);
