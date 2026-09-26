using NexoDesk.Api.Authentication;
using NexoDesk.Application.Comments.Contracts;
using NexoDesk.Application.Comments.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NexoDesk.Api.Controllers;

[ApiController]
[Route("api/tickets/{ticketId:guid}/comments")]
[Authorize]
public sealed class CommentsController(ICommentService commentService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CommentResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CommentResponse>> Create(
        Guid ticketId,
        CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var response = await commentService.CreateAsync(
            ticketId,
            User.GetUserId(),
            User.GetUserName(),
            User.GetUserRole(),
            request,
            cancellationToken);

        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<CommentResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<CommentResponse>>> GetByTicketId(
        Guid ticketId,
        CancellationToken cancellationToken)
    {
        var response = await commentService.GetByTicketIdAsync(
            ticketId,
            User.GetUserId(),
            User.GetUserRole(),
            cancellationToken);

        return Ok(response);
    }
}
