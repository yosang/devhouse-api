using devhouse.Context;
using devhouse.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using devhouse.DTOs;
namespace devhouse.Services;

public class DeveloperService
{
    public DatabaseContext _ctx { get; set; }
    public TokenService _ts { get; set; }

    public DeveloperService(DatabaseContext context, TokenService tokenService) => (_ctx, _ts) = (context, tokenService);

    public async Task<List<Developer>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        return await _ctx.Developers.AsNoTracking()
                                    .OrderBy(e => e.Id)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
    }

    public async Task<Developer> GetOne(int id) => await _ctx.Developers.Where(d => d.Id == id).FirstOrDefaultAsync() ?? null!;

    public async Task<(Developer newDev, bool unauthorized)> Create(Developer developer)
    {
        var claims = new TokenClaimsDTO { developerId = _ts.GetId(), teamId = _ts.GetTeamId(), roleId = _ts.GetRoleId() };
        if (!CanCreateDelete((RolesENUM)claims.roleId, claims, developer)) return (null!, true);

        _ctx.Developers.Add(developer);

        developer.Password = HashedPassword(developer);

        await _ctx.SaveChangesAsync();
        return (developer, false);
    }

    public async Task<(bool notFound, bool badRequest, bool unauthorized)> Update(int id, Developer developer)
    {
        var claims = new TokenClaimsDTO { developerId = _ts.GetId(), teamId = _ts.GetTeamId(), roleId = _ts.GetRoleId() };
        if (!CanModify((RolesENUM)claims.roleId, claims, developer)) return (false, false, true);

        if (id != developer.Id) return (false, true, false);

        var entity = await _ctx.Developers.FindAsync(id);
        if (entity == null) return (true, false, false);

        entity.Firstname = developer.Firstname;
        entity.Lastname = developer.Lastname;
        entity.Email = developer.Email;
        entity.Password = HashedPassword(developer);
        entity.RoleId = developer.RoleId;
        entity.TeamId = developer.TeamId;

        await _ctx.SaveChangesAsync();
        return (false, false, false);
    }

    public async Task<(bool notFound, bool unauthorized)> Delete(int id)
    {
        var entity = await _ctx.Developers.FindAsync(id);
        if (entity == null) return (true, false);

        var claims = new TokenClaimsDTO { developerId = _ts.GetId(), roleId = _ts.GetRoleId(), teamId = _ts.GetTeamId() };
        if (!CanCreateDelete((RolesENUM)claims.roleId, claims, entity)) return (false, true);

        _ctx.Remove(entity);

        await _ctx.SaveChangesAsync();
        return (false, false);
    }

    // ! We probably want to move these out to AuthService

    // Helper method for password hashing upon creation
    public string HashedPassword(Developer developer) => new PasswordHasher<Developer>().HashPassword(developer, developer.Password!);

    // Helper methods for verifying permissions based on role (RBAC - Role Based Access Control)
    public bool CanCreateDelete(RolesENUM role, TokenClaimsDTO claims, Developer developer)
        => role switch
        {
            RolesENUM.Admin => true, // can globally create/delete developers
            RolesENUM.TeamLead => claims.teamId == developer.TeamId && // can only create/delete developers within their teams
                (RolesENUM)developer.RoleId < RolesENUM.TeamLead, //  cannot create anything above Developer role level
            RolesENUM.Developer => false, // cannot create developers, only admins and teamleaders can
            _ => false
        };

    public bool CanModify(RolesENUM role, TokenClaimsDTO claims, Developer developer)
        => role switch
        {
            RolesENUM.Admin => true, // can modify anything
            RolesENUM.TeamLead => claims.teamId == developer.TeamId && // can only modify developers within their teams
                        (RolesENUM)developer.RoleId < RolesENUM.TeamLead, // can only modify developers
            RolesENUM.Developer => claims.developerId == developer.Id && // can only modify itself
                                    claims.teamId == developer.TeamId && // not allowed to modify their team id
                                    claims.roleId == developer.RoleId, // not allowed to modify their role id
            _ => false
        };

    // ENUM for role checks
    public enum RolesENUM
    {
        Developer = 1,
        TeamLead,
        Admin
    }
}