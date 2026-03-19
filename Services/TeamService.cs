using devhouse.Context;
using devhouse.DTOs;
using devhouse.Models;

using Microsoft.EntityFrameworkCore;

namespace devhouse.Services;

public class TeamService
{
    public DatabaseContext _ctx { get; set; }
    public AuthService _service { get; set; }
    public TeamService(DatabaseContext context, AuthService service) => (_ctx, _service) = (context, service);

    // ! Here we need DTO's as well
    public async Task<IEnumerable<object>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        return await _ctx.Teams.AsNoTracking()
                                .OrderBy(t => t.Id)
                                .Include(p => p.Projects)
                                .Include(d => d.Developers)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .Select(t => new
                                {
                                    t.Id,
                                    t.Name,
                                    Projects = t.Projects!.Select(p => new { p.Id, p.Name, projecttype = p.ProjectType!.Name }),
                                    Developers = t.Developers!.Select(d => new { d.Id, d.Firstname, d.Lastname, role = d.Role!.Name })
                                })
                                .ToListAsync();
    }

    // ! Replace with DTO's
    public async Task<object> GetOne(int id) => await _ctx.Teams.AsNoTracking()
                                                            .Where(t => t.Id == id)
                                                            .Include(p => p.Projects)
                                                            .Include(d => d.Developers)
                                                            .Select(t => new
                                                            {
                                                                t.Id,
                                                                t.Name,
                                                                Projects = t.Projects!.Select(p => new { p.Id, p.Name, projecttype = p.ProjectType!.Name }),
                                                                Developers = t.Developers!.Select(d => new { d.Id, d.Firstname, d.Lastname, role = d.Role!.Name })
                                                            })
                                                            .FirstOrDefaultAsync() ?? null!;

    public async Task<(Team, bool unauthorized)> Create(CreateTeamDTO dto)
    {
        var team = new Team
        {
            Name = dto.Name
        };

        if (!_service.isAdmin()) return (null!, true);

        _ctx.Teams.Add(team);
        await _ctx.SaveChangesAsync();

        return (team, false);
    }

    public async Task<(bool notFound, bool badRequest, bool unauthorized)> Update(int id, UpdateTeamDTO team)
    {
        if (id != team.Id) return (false, true, false);

        if (!_service.isAdmin()) return (false, false, true);

        var entity = await _ctx.Teams.FindAsync(id);
        if (entity == null) return (true, false, false);

        entity.Name = team.Name;

        await _ctx.SaveChangesAsync();
        return (false, false, false);
    }

    public async Task<(bool notFound, bool unauthorized)> Delete(int id)
    {
        if (!_service.isAdmin()) return (false, true);

        var entity = await _ctx.Teams.FindAsync(id);
        if (entity == null) return (true, false);

        _ctx.Remove(entity);

        await _ctx.SaveChangesAsync();
        return (false, false);
    }
}