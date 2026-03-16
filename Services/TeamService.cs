using devhouse.Context;
using devhouse.Models;

using Microsoft.EntityFrameworkCore;

namespace devhouse.Services;

public class TeamService
{
    public DatabaseContext _ctx { get; set; }

    public TeamService(DatabaseContext context) => _ctx = context;

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
}