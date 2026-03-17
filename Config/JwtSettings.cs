using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace devhouse.jwt;

public class JwtSettings
{
    public string SecretKey { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int ExpiryMinutes { get; init; }

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