namespace NexoDesk.Application.Tickets.Contracts;

public sealed class TicketListQuery
{
    public string? Status { get; init; }

    public string? Priority { get; init; }

    public bool? Assigned { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}
