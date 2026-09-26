using NexoDesk.Application.Common.Exceptions;
using NexoDesk.Application.Tickets.Contracts;
using NexoDesk.Application.Tickets.Interfaces;
using NexoDesk.Application.Tickets.Services;
using NexoDesk.Application.Tickets.Validators;
using NexoDesk.Domain.Entities;
using NexoDesk.Domain.Enums;
using Moq;

namespace NexoDesk.Tests.Application;

public sealed class TicketServiceTests
{
    [Fact]
    public async Task GetByIdAsync_WhenUserRequestsAnotherUsersTicket_ThrowsNotFound()
    {
        var repository = new Mock<ITicketRepository>();
        var ticket = CreateTicket();
        repository.Setup(item => item.GetByIdAsync(ticket.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);
        var service = CreateService(repository.Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetByIdAsync(
            ticket.Id,
            Guid.NewGuid(),
            UserRole.Usuario,
            CancellationToken.None));
    }

    [Fact]
    public async Task ChangeStatusAsync_WhenRequesterIsUser_ThrowsForbidden()
    {
        var service = CreateService(new Mock<ITicketRepository>().Object);

        await Assert.ThrowsAsync<ForbiddenException>(() => service.ChangeStatusAsync(
            Guid.NewGuid(),
            UserRole.Usuario,
            new UpdateTicketStatusRequest("RESOLVIDO"),
            CancellationToken.None));
    }

    [Fact]
    public async Task ChangePriorityAsync_WhenRequesterIsUser_ThrowsForbidden()
    {
        var service = CreateService(new Mock<ITicketRepository>().Object);

        await Assert.ThrowsAsync<ForbiddenException>(() => service.ChangePriorityAsync(
            Guid.NewGuid(),
            UserRole.Usuario,
            new UpdateTicketPriorityRequest("ALTA"),
            CancellationToken.None));
    }

    [Fact]
    public async Task AssignToCurrentAgentAsync_WhenRequesterIsAgent_AssignsTicket()
    {
        var repository = new Mock<ITicketRepository>();
        var ticket = CreateTicket();
        var agentId = Guid.NewGuid();
        repository.Setup(item => item.GetByIdForUpdateAsync(ticket.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);
        repository.Setup(item => item.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var service = CreateService(repository.Object);

        await service.AssignToCurrentAgentAsync(ticket.Id, agentId, UserRole.Agente, CancellationToken.None);

        Assert.Equal(agentId, ticket.AssignedAgentId);
        repository.Verify(item => item.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static TicketService CreateService(ITicketRepository repository) => new(
        repository,
        new CreateTicketRequestValidator(),
        new TicketListQueryValidator(),
        new UpdateTicketRequestValidator(),
        new UpdateTicketStatusRequestValidator(),
        new UpdateTicketPriorityRequestValidator());

    private static Ticket CreateTicket() => new(
        "Título válido",
        "Descrição válida para o chamado.",
        Guid.NewGuid(),
        TicketPriority.Media,
        DateTime.UtcNow);
}
