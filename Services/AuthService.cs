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

    // Takes a model entity and checks it up against the token claims
    // If it is an admin, return early with true, as Admins can modify everything
    // If the entity has implemented the interface ITeamedUp, check that both teamId and entity teamId match, TeamLeads can only modify developerss from within their teams
    // If the entity has implemented the interface ISelfModify, check the developer id, developers can only modify themselves
    public bool isAuthorized<T>(T entity)
    {
        var claims = GetClaims;
        var role = (RolesENUM)claims.roleId;

        if (isAdmin()) return true; // Admins can alter everything

        bool isTeamLead = entity is ITeamedUp t && entity is IRoledUp r ?
                            t.TeamId == claims.teamId &&
                            role == RolesENUM.TeamLead &&
                            (RolesENUM)r.RoleId < RolesENUM.TeamLead : false;

        bool isDev = entity is ISelfModify s ?
                    s.Id == claims.developerId &&
                    role == RolesENUM.Developer : false;

        return isTeamLead || isDev;
    }

    // This method instantly checks if the roleId from the token matches with the Admin enum
    // We are using this to prevent teamleads, who some permissions from altering certain properties, like RoleId and TeamId (admin protected)
    public bool isAdmin() => (RolesENUM)GetClaims.roleId == RolesENUM.Admin;

    public enum RolesENUM
    {
        Developer = 1,
        TeamLead,
        Admin
    }
}