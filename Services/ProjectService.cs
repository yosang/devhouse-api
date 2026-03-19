using devhouse.Context;
using devhouse.DTOs;
using devhouse.Models;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Services;

public class ProjectService
{
    public DatabaseContext _ctx;

    public AuthService _service { get; set; }

    public ProjectService(DatabaseContext context, AuthService service) => (_ctx, _service) = (context, service);

    public async Task<IEnumerable<object>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        // ! Replace with a dTO
        return await _ctx.Projects.AsNoTracking()
                                    .OrderBy(p => p.Id)
                                    .Include(pt => pt.ProjectType)
                                    .Include(pt => pt.Team)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(p => new
                                    {
                                        p.Id,
                                        p.Name,
                                        p.ProjectType,
                                        Team = new { p.TeamId, p.Team!.Name }
                                    })
                                    .ToListAsync();
    }

    // ! Replace with a dTO
    public async Task<object> GetOne(int id) => await _ctx.Projects.AsNoTracking()
                                                                    .Where(p => p.Id == id)
                                                                    .Include(pt => pt.ProjectType)
                                                                    .Include(pt => pt.Team)
                                                                    .Select(p => new
                                                                    {
                                                                        p.Id,
                                                                        p.Name,
                                                                        p.ProjectType,
                                                                        Team = new { p.TeamId, p.Team!.Name }
                                                                    })
                                                                    .FirstOrDefaultAsync() ?? null!;

    public async Task<(Project project, bool unauthorized)> Create(CreateProjectDTO project)
    {
        var proj = new Project
        {
            Name = project.Name,
            ProjectTypeId = project.ProjectTypeId,
            TeamId = project.TeamId
        };

        if (!_service.CanCreateModifyDeleteProjects(proj)) return (null!, true);

        _ctx.Projects.Add(proj);
        await _ctx.SaveChangesAsync();

        return (proj, false);
    }

    public async Task<(bool notFound, bool badRequest, bool unauthorized)> Update(int id, Project project)
    {
        if (id != project.Id) return (false, true, false);

        var entity = await _ctx.Projects.FindAsync(id);
        if (entity == null) return (true, false, false);

        if (!_service.CanCreateModifyDeleteProjects(entity)) return (false, false, true);

        entity.Name = project.Name;
        entity.ProjectTypeId = project.ProjectTypeId;

        if (_service.isAdmin())
        {
            entity.TeamId = project.TeamId;
        }

        await _ctx.SaveChangesAsync();

        return (false, false, false);
    }

    public async Task<(bool notFound, bool unauthorized)> Delete(int id)
    {
        var entity = await _ctx.Projects.FindAsync(id);
        if (entity == null) return (true, false);

        if (!_service.CanCreateModifyDeleteProjects(entity)) return (false, true);

        _ctx.Remove(entity);

        await _ctx.SaveChangesAsync();
        return (false, false);
    }
}