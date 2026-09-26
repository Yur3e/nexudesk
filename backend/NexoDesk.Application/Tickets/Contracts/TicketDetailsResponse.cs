namespace NexoDesk.Application.Tickets.Contracts;

public sealed record TicketDetailsResponse(
    Guid Id,
    string Title,
    string Description,
    string Status,
    string Priority,
    Guid CreatedByUserId,
    string CreatedByUserName,
    Guid? AssignedAgentId,
    string? AssignedAgentName,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? ClosedAt);
