using devhouse.Context;
using devhouse.Models;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Services;

public class ProjectService
{
    public DatabaseContext _ctx;

    public ProjectService(DatabaseContext context) => _ctx = context;

    public async Task<List<Project>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        _ = _ctx.Projects ?? throw new InvalidOperationException("Database context is malconfigured");

        return await _ctx.Projects.AsNoTracking()
                            .OrderBy(p => p.Id)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
    }
}