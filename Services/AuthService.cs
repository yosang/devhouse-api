using devhouse.Context;
using devhouse.DTOs;
using devhouse.Interfaces;
using devhouse.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Services;

public class AuthService
{
    public DatabaseContext _ctx { get; set; }
    public TokenService _ts { get; set; }

    public AuthService(DatabaseContext context, TokenService tokenService) => (_ctx, _ts) = (context, tokenService);

    TokenClaimsDTO GetClaims => new TokenClaimsDTO { roleName = _ts.GetRoleName(), developerId = _ts.GetId(), roleId = _ts.GetRoleId(), teamId = _ts.GetTeamId() };

    public async Task<(bool authenticated, string token)> Authenticate(string email, string password)
    {
        var entity = await _ctx.Developers.Include(e => e.Role) // We have to include roles so we can add the name to the JWT token
                                            .Where(e => e.Email == email)
                                            .FirstOrDefaultAsync();

        if (entity == null) return (authenticated: false, token: string.Empty);

        var isValidPassword = new PasswordHasher<Developer>().VerifyHashedPassword(entity, entity.Password!, password);

        if (isValidPassword == PasswordVerificationResult.Success)
            return (authenticated: true, token: _ts.Generate(entity));

        return (authenticated: false, token: string.Empty);
    }

    public bool isAuthorized<T>(T entity)
    {
        var claims = GetClaims;
        var role = (RolesENUM)claims.roleId;

        if (isAdmin()) return true; // Admins can alter everything

        if (entity is ITeamedUp t)
            return t.TeamId == claims.teamId && role == RolesENUM.TeamLead; // Teamleads can only alter within of their team

        if (entity is ISelfModify s)
            return s.Id == claims.developerId && role == RolesENUM.Developer; // Developers can only modify themselves

        return false;
    }

    public bool isAdmin() => (RolesENUM)GetClaims.roleId == RolesENUM.Admin;

    public enum RolesENUM
    {
        Developer = 1,
        TeamLead,
        Admin
    }
}