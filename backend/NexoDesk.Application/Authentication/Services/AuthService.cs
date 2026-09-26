using NexoDesk.Application.Authentication.Contracts;
using NexoDesk.Application.Authentication.Interfaces;
using NexoDesk.Application.Common.Exceptions;
using NexoDesk.Domain.Entities;
using NexoDesk.Domain.Enums;
using FluentValidation;

namespace NexoDesk.Application.Authentication.Services;

public sealed class AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenGenerator tokenGenerator,
    IValidator<RegisterRequest> registerRequestValidator,
    IValidator<LoginRequest> loginRequestValidator) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        await registerRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var normalizedEmail = NormalizeEmail(request.Email);

        if (await userRepository.ExistsByEmailAsync(normalizedEmail, cancellationToken))
        {
            throw new ConflictException("E-mail já está em uso.");
        }

        var user = new User(
            request.Name.Trim(),
            normalizedEmail,
            passwordHasher.Hash(request.Password),
            UserRole.Usuario,
            DateTime.UtcNow);

        await userRepository.AddAsync(user, cancellationToken);

        return CreateAuthResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        await loginRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await userRepository.GetByEmailAsync(NormalizeEmail(request.Email), cancellationToken);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException("E-mail ou senha inválidos.");
        }

        return CreateAuthResponse(user);
    }

    private AuthResponse CreateAuthResponse(User user) => new(
        user.Id,
        user.Name,
        user.Email,
        user.Role.ToString().ToUpperInvariant(),
        tokenGenerator.Generate(user));

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
