using NexoDesk.Application.Authentication.Contracts;
using FluentValidation;

namespace NexoDesk.Application.Authentication.Validators;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail deve ter um formato válido.");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.");
    }
}
