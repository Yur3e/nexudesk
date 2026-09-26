namespace NexoDesk.Application.Tickets.Contracts;

public sealed record TicketListItemResponse(
    Guid Id,
    string Title,
    string Status,
    string Priority,
    DateTime CreatedAt,
    Guid? AssignedAgentId,
    string? AssignedAgentName);
