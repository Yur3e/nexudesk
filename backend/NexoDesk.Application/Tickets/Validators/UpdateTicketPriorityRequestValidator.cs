using NexoDesk.Application.Tickets.Contracts;
using NexoDesk.Application.Tickets.Mapping;
using FluentValidation;

namespace NexoDesk.Application.Tickets.Validators;

public sealed class UpdateTicketPriorityRequestValidator : AbstractValidator<UpdateTicketPriorityRequest>
{
    public UpdateTicketPriorityRequestValidator()
    {
        RuleFor(request => request.Priority)
            .NotEmpty().WithMessage("Prioridade é obrigatória.")
            .Must(priority => TicketValueMapper.TryParsePriority(priority, out _))
            .WithMessage("Prioridade inválida.");
    }
}
