using NexoDesk.Application.Tickets.Contracts;
using NexoDesk.Application.Tickets.Interfaces;
using NexoDesk.Application.Tickets.Mapping;
using NexoDesk.Application.Tickets.Models;
using NexoDesk.Application.Common.Exceptions;
using NexoDesk.Application.Common.Models;
using NexoDesk.Domain.Entities;
using NexoDesk.Domain.Enums;
using FluentValidation;

namespace NexoDesk.Application.Tickets.Services;

public sealed class TicketService(
    ITicketRepository ticketRepository,
    IValidator<CreateTicketRequest> createTicketRequestValidator,
    IValidator<TicketListQuery> ticketListQueryValidator,
    IValidator<UpdateTicketRequest> updateTicketRequestValidator,
    IValidator<UpdateTicketStatusRequest> updateTicketStatusRequestValidator,
    IValidator<UpdateTicketPriorityRequest> updateTicketPriorityRequestValidator) : ITicketService
{
    public async Task<TicketResponse> CreateAsync(
        Guid createdByUserId,
        CreateTicketRequest request,
        CancellationToken cancellationToken)
    {
        await createTicketRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var ticket = new Ticket(
            request.Title.Trim(),
            request.Description.Trim(),
            createdByUserId,
            TicketPriority.Media,
            DateTime.UtcNow);

        await ticketRepository.AddAsync(ticket, cancellationToken);

        return Map(ticket);
    }

    public async Task<PagedResponse<TicketListItemResponse>> GetAllAsync(
        Guid requestingUserId,
        UserRole requestingUserRole,
        TicketListQuery query,
        CancellationToken cancellationToken)
    {
        await ticketListQueryValidator.ValidateAndThrowAsync(query, cancellationToken);

        Guid? createdByUserId = requestingUserRole switch
        {
            UserRole.Usuario => requestingUserId,
            UserRole.Agente => null,
            _ => throw new UnauthorizedAccessException("Perfil de acesso inválido.")
        };

        TicketStatus? status = null;
        TicketPriority? priority = null;

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            TicketValueMapper.TryParseStatus(query.Status, out var parsedStatus);
            status = parsedStatus;
        }

        if (!string.IsNullOrWhiteSpace(query.Priority))
        {
            TicketValueMapper.TryParsePriority(query.Priority, out var parsedPriority);
            priority = parsedPriority;
        }

        var filter = new TicketFilter(status, priority, query.Assigned, query.Page, query.PageSize);
        var result = await ticketRepository.GetPageAsync(createdByUserId, filter, cancellationToken);
        var totalPages = (int)Math.Ceiling(result.TotalItems / (double)query.PageSize);

        return new PagedResponse<TicketListItemResponse>(
            result.Items.Select(MapListItem).ToList(),
            query.Page,
            query.PageSize,
            result.TotalItems,
            totalPages);
    }

    public async Task<TicketDetailsResponse> GetByIdAsync(
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
            throw new UnauthorizedAccessException("Perfil de acesso inválido.");
        }

        return MapDetails(ticket);
    }

    public async Task<TicketDetailsResponse> UpdateAsync(
        Guid ticketId,
        Guid requestingUserId,
        UpdateTicketRequest request,
        CancellationToken cancellationToken)
    {
        await updateTicketRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var ticket = await GetTicketForUpdateAsync(ticketId, cancellationToken);

        if (ticket.CreatedByUserId != requestingUserId)
        {
            throw new KeyNotFoundException("Ticket não encontrado.");
        }

        ticket.UpdateDetails(request.Title.Trim(), request.Description.Trim(), DateTime.UtcNow);
        await ticketRepository.SaveChangesAsync(cancellationToken);

        return MapDetails(ticket);
    }

    public async Task<TicketDetailsResponse> ChangeStatusAsync(
        Guid ticketId,
        UserRole requestingUserRole,
        UpdateTicketStatusRequest request,
        CancellationToken cancellationToken)
    {
        EnsureAgent(requestingUserRole);
        await updateTicketStatusRequestValidator.ValidateAndThrowAsync(request, cancellationToken);
        TicketValueMapper.TryParseStatus(request.Status, out var status);

        var ticket = await GetTicketForUpdateAsync(ticketId, cancellationToken);
        ticket.ChangeStatus(status, DateTime.UtcNow);
        await ticketRepository.SaveChangesAsync(cancellationToken);

        return MapDetails(ticket);
    }

    public async Task<TicketDetailsResponse> ChangePriorityAsync(
        Guid ticketId,
        UserRole requestingUserRole,
        UpdateTicketPriorityRequest request,
        CancellationToken cancellationToken)
    {
        EnsureAgent(requestingUserRole);
        await updateTicketPriorityRequestValidator.ValidateAndThrowAsync(request, cancellationToken);
        TicketValueMapper.TryParsePriority(request.Priority, out var priority);

        var ticket = await GetTicketForUpdateAsync(ticketId, cancellationToken);
        ticket.ChangePriority(priority, DateTime.UtcNow);
        await ticketRepository.SaveChangesAsync(cancellationToken);

        return MapDetails(ticket);
    }

    public async Task<TicketDetailsResponse> AssignToCurrentAgentAsync(
        Guid ticketId,
        Guid agentId,
        UserRole requestingUserRole,
        CancellationToken cancellationToken)
    {
        EnsureAgent(requestingUserRole);

        var ticket = await GetTicketForUpdateAsync(ticketId, cancellationToken);
        ticket.AssignTo(agentId, DateTime.UtcNow);
        await ticketRepository.SaveChangesAsync(cancellationToken);

        return MapDetails(ticket);
    }

    private static TicketResponse Map(Ticket ticket) => new(
        ticket.Id,
        ticket.Title,
        ticket.Description,
        TicketValueMapper.ToApiValue(ticket.Status),
        TicketValueMapper.ToApiValue(ticket.Priority),
        ticket.CreatedByUserId,
        ticket.AssignedAgentId,
        ticket.CreatedAt,
        ticket.UpdatedAt,
        ticket.ClosedAt);

    private static TicketListItemResponse MapListItem(Ticket ticket) => new(
        ticket.Id,
        ticket.Title,
        TicketValueMapper.ToApiValue(ticket.Status),
        TicketValueMapper.ToApiValue(ticket.Priority),
        ticket.CreatedAt,
        ticket.AssignedAgentId,
        ticket.AssignedAgent?.Name);

    private static TicketDetailsResponse MapDetails(Ticket ticket) => new(
        ticket.Id,
        ticket.Title,
        ticket.Description,
        TicketValueMapper.ToApiValue(ticket.Status),
        TicketValueMapper.ToApiValue(ticket.Priority),
        ticket.CreatedByUserId,
        ticket.CreatedByUser?.Name ?? string.Empty,
        ticket.AssignedAgentId,
        ticket.AssignedAgent?.Name,
        ticket.CreatedAt,
        ticket.UpdatedAt,
        ticket.ClosedAt);

    private async Task<Ticket> GetTicketForUpdateAsync(Guid ticketId, CancellationToken cancellationToken) =>
        await ticketRepository.GetByIdForUpdateAsync(ticketId, cancellationToken)
        ?? throw new KeyNotFoundException("Ticket não encontrado.");

    private static void EnsureAgent(UserRole userRole)
    {
        if (userRole != UserRole.Agente)
        {
            throw new ForbiddenException("Apenas agentes podem executar esta ação.");
        }
    }
}
