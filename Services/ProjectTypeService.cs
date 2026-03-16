using devhouse.Context;
using devhouse.Models;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Services;

public class ProjectTypeService
{
    public DatabaseContext _ctx { get; set; }
    public ProjectTypeService(DatabaseContext context) => _ctx = context;

    public async Task<List<ProjectType>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        _ = _ctx.ProjectTypes ?? throw new InvalidOperationException("Database context is malconfigured");

        return await _ctx.ProjectTypes.AsNoTracking() // No need to track all these entities as we wont mutate them here, we save some performance here
                                        .OrderBy(pt => pt.Id)
                                        .Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();
    }
}