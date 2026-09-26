using NexoDesk.Domain.Enums;

namespace NexoDesk.Domain.Entities;

public sealed class Ticket
{
    private Ticket()
    {
    }

    public Ticket(
        string title,
        string description,
        Guid createdByUserId,
        TicketPriority priority,
        DateTime createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentOutOfRangeException.ThrowIfEqual(createdByUserId, Guid.Empty);

        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        CreatedByUserId = createdByUserId;
        Priority = priority;
        Status = TicketStatus.Aberto;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public TicketStatus Status { get; private set; }

    public TicketPriority Priority { get; private set; }

    public Guid CreatedByUserId { get; private set; }

    public Guid? AssignedAgentId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public DateTime? ClosedAt { get; private set; }

    public User CreatedByUser { get; private set; } = null!;

    public User? AssignedAgent { get; private set; }

    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();

    public void UpdateDetails(string title, string description, DateTime updatedAt)
    {
        EnsureIsNotClosed();
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        Title = title;
        Description = description;
        UpdatedAt = updatedAt;
    }

    public void ChangeStatus(TicketStatus status, DateTime updatedAt)
    {
        EnsureIsNotClosed();

        Status = status;
        UpdatedAt = updatedAt;

        if (status == TicketStatus.Fechado)
        {
            ClosedAt = updatedAt;
        }
    }

    public void ChangePriority(TicketPriority priority, DateTime updatedAt)
    {
        EnsureIsNotClosed();

        Priority = priority;
        UpdatedAt = updatedAt;
    }

    public void AssignTo(Guid agentId, DateTime updatedAt)
    {
        EnsureIsNotClosed();
        ArgumentOutOfRangeException.ThrowIfEqual(agentId, Guid.Empty);

        AssignedAgentId = agentId;
        UpdatedAt = updatedAt;
    }

    public void EnsureCanReceiveComments()
    {
        EnsureIsNotClosed();
    }

    private void EnsureIsNotClosed()
    {
        if (Status == TicketStatus.Fechado)
        {
            throw new InvalidOperationException("Closed tickets cannot be changed.");
        }
    }
}
