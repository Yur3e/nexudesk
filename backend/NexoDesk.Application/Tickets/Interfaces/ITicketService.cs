using NexoDesk.Application.Tickets.Contracts;
using NexoDesk.Application.Common.Models;
using NexoDesk.Domain.Enums;

namespace NexoDesk.Application.Tickets.Interfaces;

public interface ITicketService
{
    Task<TicketResponse> CreateAsync(
        Guid createdByUserId,
        CreateTicketRequest request,
        CancellationToken cancellationToken);

    Task<PagedResponse<TicketListItemResponse>> GetAllAsync(
        Guid requestingUserId,
        UserRole requestingUserRole,
        TicketListQuery query,
        CancellationToken cancellationToken);

    Task<TicketDetailsResponse> GetByIdAsync(
        Guid ticketId,
        Guid requestingUserId,
        UserRole requestingUserRole,
        CancellationToken cancellationToken);

    Task<TicketDetailsResponse> UpdateAsync(
        Guid ticketId,
        Guid requestingUserId,
        UpdateTicketRequest request,
        CancellationToken cancellationToken);

    Task<TicketDetailsResponse> ChangeStatusAsync(
        Guid ticketId,
        UserRole requestingUserRole,
        UpdateTicketStatusRequest request,
        CancellationToken cancellationToken);

    Task<TicketDetailsResponse> ChangePriorityAsync(
        Guid ticketId,
        UserRole requestingUserRole,
        UpdateTicketPriorityRequest request,
        CancellationToken cancellationToken);

    Task<TicketDetailsResponse> AssignToCurrentAgentAsync(
        Guid ticketId,
        Guid agentId,
        UserRole requestingUserRole,
        CancellationToken cancellationToken);
}
