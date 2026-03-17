using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using devhouse.jwt;
using Microsoft.IdentityModel.Tokens;

namespace devhouse.Services;

public class TokenService
{
    public JwtSettings _jwtSettings { get; set; }
    public TokenService(JwtSettings jwt) => _jwtSettings = jwt;

    public string Generate(string name)
        => new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        claims: new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
        },
        expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
            SecurityAlgorithms.HmacSha256
        )
    ));
}