using NexoDesk.Domain.Entities;

namespace NexoDesk.Application.Comments.Interfaces;

public interface ICommentRepository
{
    Task AddAsync(Comment comment, CancellationToken cancellationToken);

    Task<IReadOnlyList<Comment>> GetByTicketIdAsync(Guid ticketId, CancellationToken cancellationToken);
}
