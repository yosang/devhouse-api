using devhouse.Context;
using devhouse.Models;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Services;

public class ProjectService
{
    public DatabaseContext _ctx;

    public AuthService _service { get; set; }

    public ProjectService(DatabaseContext context, AuthService service) => (_ctx, _service) = (context, service);

    public async Task<List<Project>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        return await _ctx.Projects.AsNoTracking()
                            .OrderBy(p => p.Id)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
    }

    public async Task<Project> GetOne(int id) => await _ctx.Projects.Where(p => p.Id == id).FirstOrDefaultAsync() ?? null!;

    public async Task<(Project project, bool unauthorized)> Create(Project project)
    {
        // if (!_service.isAdmin()) return (null!, true);

        _ctx.Projects.Add(project);
        await _ctx.SaveChangesAsync();

        return (project, false);
    }

    public async Task<(bool notFound, bool badRequest)> Update(int id, Project project)
    {
        if (id != project.Id) return (false, true);

        var entity = await _ctx.Projects.FindAsync(id);
        if (entity == null) return (true, false);

        // ! Check if permissions pass

        entity.Name = project.Name;
        entity.TeamId = project.TeamId;
        entity.ProjectTypeId = project.ProjectTypeId;

        await _ctx.SaveChangesAsync();

        return (false, false);
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await _ctx.Projects.FindAsync(id);
        if (entity == null) return false;

        // ! Check if permissions pass

        _ctx.Remove(entity);

        await _ctx.SaveChangesAsync();
        return true;
    }
}