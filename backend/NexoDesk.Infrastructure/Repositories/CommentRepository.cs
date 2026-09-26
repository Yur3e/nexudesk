using NexoDesk.Application.Comments.Interfaces;
using NexoDesk.Domain.Entities;
using NexoDesk.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace NexoDesk.Infrastructure.Repositories;

public sealed class CommentRepository(HelpDeskDbContext context) : ICommentRepository
{
    public async Task AddAsync(Comment comment, CancellationToken cancellationToken)
    {
        await context.Comments.AddAsync(comment, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Comment>> GetByTicketIdAsync(Guid ticketId, CancellationToken cancellationToken) =>
        await context.Comments
            .AsNoTracking()
            .Include(comment => comment.User)
            .Where(comment => comment.TicketId == ticketId)
            .OrderBy(comment => comment.CreatedAt)
            .ToListAsync(cancellationToken);
}
