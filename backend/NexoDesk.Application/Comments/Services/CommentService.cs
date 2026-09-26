using NexoDesk.Application.Comments.Contracts;
using NexoDesk.Application.Comments.Interfaces;
using NexoDesk.Application.Common.Exceptions;
using NexoDesk.Application.Tickets.Interfaces;
using NexoDesk.Domain.Entities;
using NexoDesk.Domain.Enums;
using FluentValidation;

namespace NexoDesk.Application.Comments.Services;

public sealed class CommentService(
    ICommentRepository commentRepository,
    ITicketRepository ticketRepository,
    IValidator<CreateCommentRequest> createCommentRequestValidator) : ICommentService
{
    public async Task<CommentResponse> CreateAsync(
        Guid ticketId,
        Guid requestingUserId,
        string requestingUserName,
        UserRole requestingUserRole,
        CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        await createCommentRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var ticket = await GetAccessibleTicketAsync(
            ticketId,
            requestingUserId,
            requestingUserRole,
            cancellationToken);

        ticket.EnsureCanReceiveComments();

        var comment = new Comment(request.Content.Trim(), ticketId, requestingUserId, DateTime.UtcNow);
        await commentRepository.AddAsync(comment, cancellationToken);

        return new CommentResponse(
            comment.Id,
            comment.Content,
            comment.TicketId,
            comment.UserId,
            requestingUserName,
            comment.CreatedAt);
    }

    public async Task<IReadOnlyList<CommentResponse>> GetByTicketIdAsync(
        Guid ticketId,
        Guid requestingUserId,
        UserRole requestingUserRole,
        CancellationToken cancellationToken)
    {
        await GetAccessibleTicketAsync(ticketId, requestingUserId, requestingUserRole, cancellationToken);

        var comments = await commentRepository.GetByTicketIdAsync(ticketId, cancellationToken);

        return comments.Select(comment => new CommentResponse(
            comment.Id,
            comment.Content,
            comment.TicketId,
            comment.UserId,
            comment.User.Name,
            comment.CreatedAt)).ToList();
    }

    private async Task<Ticket> GetAccessibleTicketAsync(
        Guid ticketId,
        Guid requestingUserId,
        UserRole requestingUserRole,
        CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(ticketId, cancellationToken)
            ?? throw new KeyNotFoundException("Ticket não encontrado.");

        if (requestingUserRole == UserRole.Usuario && ticket.CreatedByUserId != requestingUserId)
        {
            throw new KeyNotFoundException("Ticket não encontrado.");
        }

        if (requestingUserRole is not (UserRole.Usuario or UserRole.Agente))
        {
            throw new ForbiddenException("Perfil de acesso inválido.");
        }

        return ticket;
    }
}
