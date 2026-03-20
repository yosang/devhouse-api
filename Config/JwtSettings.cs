using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace devhouse.jwt;

/// <summary>Configures JWT settings which is mapped from appsettings.json</summary>
public class JwtSettings
{
    public string SecretKey { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; }

    /// <summary>Creates a new instance of the TokenValidationParameters class needed for validating JWT tokens from the request Authorization header</summary>
    public TokenValidationParameters tokenValidationParameters
    {
        get
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey))
            };
        }
    }
}