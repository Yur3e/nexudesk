using System.Security.Claims;
using NexoDesk.Domain.Enums;

namespace NexoDesk.Api.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Token de acesso inválido.");
        }

        return userId;
    }

    public static UserRole GetUserRole(this ClaimsPrincipal user)
    {
        var roleClaim = user.FindFirstValue(ClaimTypes.Role);

        if (!Enum.TryParse<UserRole>(roleClaim, ignoreCase: true, out var role))
        {
            throw new UnauthorizedAccessException("Token de acesso inválido.");
        }

        return role;
    }

    public static string GetUserName(this ClaimsPrincipal user)
    {
        var userName = user.FindFirstValue(ClaimTypes.Name);

        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new UnauthorizedAccessException("Token de acesso inválido.");
        }

        return userName;
    }
}
