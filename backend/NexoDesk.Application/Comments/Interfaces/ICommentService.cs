using NexoDesk.Application.Comments.Contracts;
using NexoDesk.Domain.Enums;

namespace NexoDesk.Application.Comments.Interfaces;

public interface ICommentService
{
    Task<CommentResponse> CreateAsync(
        Guid ticketId,
        Guid requestingUserId,
        string requestingUserName,
        UserRole requestingUserRole,
        CreateCommentRequest request,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<CommentResponse>> GetByTicketIdAsync(
        Guid ticketId,
        Guid requestingUserId,
        UserRole requestingUserRole,
        CancellationToken cancellationToken);
}
