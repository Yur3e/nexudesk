using NexoDesk.Application.Tickets.Contracts;
using NexoDesk.Application.Tickets.Mapping;
using FluentValidation;

namespace NexoDesk.Application.Tickets.Validators;

public sealed class UpdateTicketStatusRequestValidator : AbstractValidator<UpdateTicketStatusRequest>
{
    public UpdateTicketStatusRequestValidator()
    {
        RuleFor(request => request.Status)
            .NotEmpty().WithMessage("Status é obrigatório.")
            .Must(status => TicketValueMapper.TryParseStatus(status, out _))
            .WithMessage("Status inválido.");
    }
}
