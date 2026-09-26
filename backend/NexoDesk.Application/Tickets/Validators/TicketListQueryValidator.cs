using NexoDesk.Application.Tickets.Contracts;
using NexoDesk.Application.Tickets.Mapping;
using FluentValidation;

namespace NexoDesk.Application.Tickets.Validators;

public sealed class TicketListQueryValidator : AbstractValidator<TicketListQuery>
{
    public TicketListQueryValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Página deve ser maior ou igual a 1.");

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Tamanho da página deve estar entre 1 e 100.");

        When(query => !string.IsNullOrWhiteSpace(query.Status), () =>
            RuleFor(query => query.Status)
                .Must(status => TicketValueMapper.TryParseStatus(status, out _))
                .WithMessage("Status inválido."));

        When(query => !string.IsNullOrWhiteSpace(query.Priority), () =>
            RuleFor(query => query.Priority)
                .Must(priority => TicketValueMapper.TryParsePriority(priority, out _))
                .WithMessage("Prioridade inválida."));
    }
}
