using devhouse.Context;
using devhouse.Models;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Services;

public class ProjectTypeService
{
    public DatabaseContext _ctx { get; set; }
    public AuthService _service { get; set; }
    public ProjectTypeService(DatabaseContext context, AuthService service) => (_ctx, _service) = (context, service);

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

    public async Task<(ProjectType project, bool unauthorized)> Create(ProjectType pt)
    {
        if (!_service.isAdmin()) return (null!, true);

        _ctx.ProjectTypes.Add(pt);
        await _ctx.SaveChangesAsync();
        return (pt, false);
    }

    public async Task<(bool notFound, bool badRequest, bool unauthorized)> Update(int id, ProjectType pt)
    {
        if (id != pt.Id) return (false, true, false);

        if (!_service.isAdmin()) return (false, false, true);

        var entity = await _ctx.ProjectTypes.FindAsync(id);
        if (entity == null) return (true, false, false);


        entity.Name = pt.Name;

        await _ctx.SaveChangesAsync();
        return (false, false, false);
    }

    public async Task<(bool notFound, bool unauthorized)> Delete(int id)
    {
        if (!_service.isAdmin()) return (false, true);

        var entity = await _ctx.ProjectTypes.FindAsync(id);
        if (entity == null) return (true, false);

        _ctx.Remove(entity);

        await _ctx.SaveChangesAsync();
        return (false, false);
    }
}