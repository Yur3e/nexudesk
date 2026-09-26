using NexoDesk.Application.Authentication.Contracts;

namespace NexoDesk.Application.Authentication.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);

    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
}
