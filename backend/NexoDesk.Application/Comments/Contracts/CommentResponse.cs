namespace NexoDesk.Application.Comments.Contracts;

public sealed record CommentResponse(
    Guid Id,
    string Content,
    Guid TicketId,
    Guid UserId,
    string UserName,
    DateTime CreatedAt);
