namespace NexoDesk.Domain.Entities;

public sealed class Comment
{
    private Comment()
    {
    }

    public Comment(string content, Guid ticketId, Guid userId, DateTime createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);
        ArgumentOutOfRangeException.ThrowIfEqual(ticketId, Guid.Empty);
        ArgumentOutOfRangeException.ThrowIfEqual(userId, Guid.Empty);

        Id = Guid.NewGuid();
        Content = content;
        TicketId = ticketId;
        UserId = userId;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }

    public string Content { get; private set; } = null!;

    public Guid TicketId { get; private set; }

    public Guid UserId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Ticket Ticket { get; private set; } = null!;

    public User User { get; private set; } = null!;
}
