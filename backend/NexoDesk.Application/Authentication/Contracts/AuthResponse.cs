namespace NexoDesk.Application.Authentication.Contracts;

public sealed record AuthResponse(Guid UserId, string Name, string Email, string Role, string AccessToken);
