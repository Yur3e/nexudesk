using System.Text;

namespace NexoDesk.Infrastructure.Authentication;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string Key { get; set; } = string.Empty;

    public int ExpirationMinutes { get; set; } = 60;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Issuer) || string.IsNullOrWhiteSpace(Audience))
        {
            throw new InvalidOperationException("JWT issuer and audience must be configured.");
        }

        if (Encoding.UTF8.GetByteCount(Key) < 32)
        {
            throw new InvalidOperationException("JWT key must have at least 32 bytes.");
        }

        if (ExpirationMinutes <= 0)
        {
            throw new InvalidOperationException("JWT expiration must be greater than zero.");
        }
    }
}
