using NexoDesk.Domain.Entities;
using NexoDesk.Domain.Enums;

namespace NexoDesk.Tests.Domain;

public sealed class TicketTests
{
    [Fact]
    public void ChangeStatus_ToClosed_SetsClosedAt()
    {
        var ticket = CreateTicket();
        var closedAt = DateTime.UtcNow;

        ticket.ChangeStatus(TicketStatus.Fechado, closedAt);

        Assert.Equal(TicketStatus.Fechado, ticket.Status);
        Assert.Equal(closedAt, ticket.ClosedAt);
    }

    [Fact]
    public void ClosedTicket_CannotBeUpdated()
    {
        var ticket = CreateTicket();
        ticket.ChangeStatus(TicketStatus.Fechado, DateTime.UtcNow);

        Assert.Throws<InvalidOperationException>(() =>
            ticket.UpdateDetails("Novo título", "Nova descrição válida.", DateTime.UtcNow));
    }

    [Fact]
    public void ClosedTicket_CannotReceiveComments()
    {
        var ticket = CreateTicket();
        ticket.ChangeStatus(TicketStatus.Fechado, DateTime.UtcNow);

        Assert.Throws<InvalidOperationException>(ticket.EnsureCanReceiveComments);
    }

    private static Ticket CreateTicket() => new(
        "Título válido",
        "Descrição válida para o chamado.",
        Guid.NewGuid(),
        TicketPriority.Media,
        DateTime.UtcNow);
}
