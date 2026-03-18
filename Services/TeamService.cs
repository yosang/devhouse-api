using devhouse.Context;
using devhouse.Models;

using Microsoft.EntityFrameworkCore;

namespace devhouse.Services;

public class TeamService
{
    public DatabaseContext _ctx { get; set; }
    public AuthService _service { get; set; }
    public TeamService(DatabaseContext context, AuthService service) => (_ctx, _service) = (context, service);

    public async Task<List<Team>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        return await _ctx.Teams.AsNoTracking()
                                .OrderBy(r => r.Id)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
    }

    public async Task<Team> GetOne(int id) => await _ctx.Teams.Where(t => t.Id == id).FirstOrDefaultAsync() ?? null!;

    public async Task<(Team, bool unauthorized)> Create(Team team)
    {
        if (!_service.isAdmin()) return (null!, true);

        _ctx.Teams.Add(team);
        await _ctx.SaveChangesAsync();

        return (team, false);
    }

    public async Task<(bool notFound, bool badRequest, bool unauthorized)> Update(int id, Team team)
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