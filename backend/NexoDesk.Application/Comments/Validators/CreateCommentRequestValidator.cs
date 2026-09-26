using NexoDesk.Application.Comments.Contracts;
using FluentValidation;

namespace NexoDesk.Application.Comments.Validators;

public sealed class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(request => request.Content)
            .NotEmpty().WithMessage("Conteúdo do comentário é obrigatório.");
    }
}
