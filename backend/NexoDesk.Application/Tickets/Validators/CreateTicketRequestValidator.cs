using NexoDesk.Application.Tickets.Contracts;
using FluentValidation;

namespace NexoDesk.Application.Tickets.Validators;

public sealed class CreateTicketRequestValidator : AbstractValidator<CreateTicketRequest>
{
    public CreateTicketRequestValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty().WithMessage("Título é obrigatório.")
            .Length(3, 150).WithMessage("Título deve ter entre 3 e 150 caracteres.");

        RuleFor(request => request.Description)
            .NotEmpty().WithMessage("Descrição é obrigatória.")
            .Length(10, 3000).WithMessage("Descrição deve ter entre 10 e 3000 caracteres.");
    }
}
