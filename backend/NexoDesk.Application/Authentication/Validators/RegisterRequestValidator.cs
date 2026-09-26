using NexoDesk.Application.Authentication.Contracts;
using FluentValidation;

namespace NexoDesk.Application.Authentication.Validators;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.");

        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail deve ter um formato válido.");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres.");
    }
}
