using NexoDesk.Api.Authentication;
using NexoDesk.Application.Common.Models;
using NexoDesk.Application.Tickets.Contracts;
using NexoDesk.Application.Tickets.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NexoDesk.Api.Controllers;

[ApiController]
[Route("api/tickets")]
[Authorize]
public sealed class TicketsController(ITicketService ticketService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<PagedResponse<TicketListItemResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResponse<TicketListItemResponse>>> GetAll(
        [FromQuery] TicketListQuery query,
        CancellationToken cancellationToken)
    {
        var response = await ticketService.GetAllAsync(
            User.GetUserId(),
            User.GetUserRole(),
            query,
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("{ticketId:guid}")]
    [ProducesResponseType<TicketDetailsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketDetailsResponse>> GetById(Guid ticketId, CancellationToken cancellationToken)
    {
        var response = await ticketService.GetByIdAsync(
            ticketId,
            User.GetUserId(),
            User.GetUserRole(),
            cancellationToken);

        return Ok(response);
    }

    [HttpPut("{ticketId:guid}")]
    [ProducesResponseType<TicketDetailsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TicketDetailsResponse>> Update(
        Guid ticketId,
        UpdateTicketRequest request,
        CancellationToken cancellationToken)
    {
        var response = await ticketService.UpdateAsync(
            ticketId,
            User.GetUserId(),
            request,
            cancellationToken);

        return Ok(response);
    }

    [HttpPatch("{ticketId:guid}/status")]
    [Authorize(Roles = "AGENTE")]
    [ProducesResponseType<TicketDetailsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TicketDetailsResponse>> ChangeStatus(
        Guid ticketId,
        UpdateTicketStatusRequest request,
        CancellationToken cancellationToken)
    {
        var response = await ticketService.ChangeStatusAsync(
            ticketId,
            User.GetUserRole(),
            request,
            cancellationToken);

        return Ok(response);
    }

    [HttpPatch("{ticketId:guid}/priority")]
    [Authorize(Roles = "AGENTE")]
    [ProducesResponseType<TicketDetailsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TicketDetailsResponse>> ChangePriority(
        Guid ticketId,
        UpdateTicketPriorityRequest request,
        CancellationToken cancellationToken)
    {
        var response = await ticketService.ChangePriorityAsync(
            ticketId,
            User.GetUserRole(),
            request,
            cancellationToken);

        return Ok(response);
    }

    [HttpPatch("{ticketId:guid}/assign")]
    [Authorize(Roles = "AGENTE")]
    [ProducesResponseType<TicketDetailsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TicketDetailsResponse>> AssignToCurrentAgent(
        Guid ticketId,
        CancellationToken cancellationToken)
    {
        var response = await ticketService.AssignToCurrentAgentAsync(
            ticketId,
            User.GetUserId(),
            User.GetUserRole(),
            cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType<TicketResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TicketResponse>> Create(
        CreateTicketRequest request,
        CancellationToken cancellationToken)
    {
        var response = await ticketService.CreateAsync(
            User.GetUserId(),
            request,
            cancellationToken);

        return StatusCode(StatusCodes.Status201Created, response);
    }
}
