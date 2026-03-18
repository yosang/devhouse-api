using devhouse.Context;
using devhouse.DTOs;
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

    public bool CanCreateOrDeleteDeveloper(Developer developer)
    {
        var claims = GetClaims;
        var role = (RolesENUM)claims.roleId;

        return role switch
        {
            RolesENUM.Admin => true, // can globally create/delete developers
            RolesENUM.TeamLead => claims.teamId == developer.TeamId,// can only create/delete developers within their teams
            RolesENUM.Developer => false, // cannot create developers, only admins and teamleaders can
            _ => false
        };
    }

    public bool CanModifyDeveloper(Developer developer)
    {
        var claims = GetClaims;
        var role = (RolesENUM)claims.roleId;

        return role switch
        {
            RolesENUM.Admin => true, // can modify anything
            RolesENUM.TeamLead => claims.teamId == developer.TeamId, // can only modify developers within their teams
            RolesENUM.Developer => claims.developerId == developer.Id,// can only modify itself
            _ => false
        };
    }

    public bool CanCreateModifyOrDeleteProject(Project project)
    {
        var claims = GetClaims;
        var role = (RolesENUM)GetClaims.roleId;

        return role switch
        {
            RolesENUM.Admin => true,
            RolesENUM.TeamLead => claims.teamId == project.TeamId,// can only create/modidfy or delete projects within their teams
            RolesENUM.Developer => false, // cannot create projects, only admins and teamleaders can
            _ => false
        };
    }

    public bool isAdmin() => GetClaims.roleName == RolesENUM.Admin.ToString();

    public enum RolesENUM
    {
        Developer = 1,
        TeamLead,
        Admin
    }
}