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

        return await _ctx.ProjectTypes.AsNoTracking() // No need to track all these entities as we wont mutate them here, we save some performance here
                                        .OrderBy(pt => pt.Id)
                                        .Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();
    }

    public async Task<ProjectType> GetOne(int id) => await _ctx.ProjectTypes.Where(pt => pt.Id == id).FirstOrDefaultAsync() ?? null!;

    public async Task<ProjectType> Create(ProjectType pt)
    {
        _ctx.ProjectTypes.Add(pt);
        await _ctx.SaveChangesAsync();
        return pt;
    }

    public async Task<bool> Update(int id, ProjectType pt)
    {
        // To be implemented
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        // To be implemented
        return true;
    }
}