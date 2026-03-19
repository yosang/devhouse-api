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

    // Takes a developer entity and checks it up against the token claims
    // If it is an admin, return early with true, as Admins can modify everything
    // If its a teamlead check that both teamId and entity teamId match, TeamLeads can only modify developers from within their teams
    // If tits a developer check the developer id, developers can only modify themselves
    public bool CanCreateDeleteDevelopers(Developer developer)
    {
        var claims = GetClaims;

        return (RolesENUM)claims.roleId switch
        {
            RolesENUM.Admin => true,
            RolesENUM.TeamLead => developer.TeamId == claims.teamId && (RolesENUM)developer.RoleId < RolesENUM.TeamLead,
            RolesENUM.Developer => false,
            _ => false
        };

    }

    public bool CanModifyDevelopers(Developer developer)
    {
        var claims = GetClaims;

        return (RolesENUM)claims.roleId switch
        {
            RolesENUM.Admin => true,
            RolesENUM.TeamLead => developer.TeamId == claims.teamId && (RolesENUM)developer.RoleId < RolesENUM.TeamLead,
            RolesENUM.Developer => developer.Id == claims.developerId,
            _ => false
        };
    }

    public bool CanCreateModifyDeleteProjects(Project project)
    {
        var claims = GetClaims;
        return (RolesENUM)claims.roleId switch
        {
            RolesENUM.Admin => true,
            RolesENUM.TeamLead => project.TeamId == claims.teamId && (RolesENUM)project.TeamId < RolesENUM.TeamLead,
            _ => false
        };
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

    public string HashedPassword(string password) => new PasswordHasher<object>().HashPassword(new object(), password);
}