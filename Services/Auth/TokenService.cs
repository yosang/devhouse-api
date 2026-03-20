using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using devhouse.jwt;
using devhouse.Models;
using Microsoft.IdentityModel.Tokens;

namespace devhouse.Services;

public class TokenService
{
    public JwtSettings _jwtSettings { get; set; }
    public IHttpContextAccessor _http { get; set; }

    public TokenService(JwtSettings jwt, IHttpContextAccessor http) => (_jwtSettings, _http) = (jwt, http);

    /// <summary>Generates a new JWT token</summary>
    /// <param name="developer"></param>
    /// <returns>Serialized string token</returns>
    public string Generate(Developer developer)
        => new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        claims: new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, developer.Id.ToString()!),
            new Claim(ClaimTypes.Role, developer.Role!.ToString()!),
            new Claim("teamId", developer.TeamId.ToString()),
            new Claim("roleId", developer.RoleId.ToString()),
        },

        expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)), SecurityAlgorithms.HmacSha256)
    ));

    // These are helpers that retrieve certain values from the JWT token using HttpContext
    public string GetRoleName() => _http.HttpContext!.User.FindFirstValue(ClaimTypes.Role)!;
    public int GetId() => Convert.ToInt32(_http.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
    public int GetRoleId() => Convert.ToInt32(_http.HttpContext!.User.FindFirstValue("roleId"));
    public int GetTeamId() => Convert.ToInt32(_http.HttpContext!.User.FindFirstValue("teamId"));
}