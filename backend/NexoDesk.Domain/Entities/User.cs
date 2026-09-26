using NexoDesk.Domain.Enums;

namespace NexoDesk.Domain.Entities;

public sealed class User
{
    private User()
    {
    }

    public User(string name, string email, string passwordHash, UserRole role, DateTime createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public UserRole Role { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public ICollection<Ticket> CreatedTickets { get; private set; } = new List<Ticket>();

    public ICollection<Ticket> AssignedTickets { get; private set; } = new List<Ticket>();

    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();

    public void UpdateName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name.Trim();
    }
}
