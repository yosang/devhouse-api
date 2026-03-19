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

    public async Task<IEnumerable<ReadProjectDTO>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        return await _ctx.Projects.AsNoTracking()
                                    .OrderBy(p => p.Id)
                                    .Include(pt => pt.ProjectType)
                                    .Include(pt => pt.Team)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(p => new ReadProjectDTO
                                    {
                                        Id = p.Id,
                                        Name = p.Name,
                                        ProjectType = p.ProjectType!.Name,
                                        Team = new TeamDTO { Id = p.TeamId, Name = p.Team!.Name }
                                    })
                                    .ToListAsync();
    }

    public async Task<ReadProjectDTO> GetOne(int id) => await _ctx.Projects.AsNoTracking()
                                                                    .Where(p => p.Id == id)
                                                                    .Include(pt => pt.ProjectType)
                                                                    .Include(pt => pt.Team)
                                                                    .Select(p => new ReadProjectDTO
                                                                    {
                                                                        Id = p.Id,
                                                                        Name = p.Name,
                                                                        ProjectType = p.ProjectType!.Name,
                                                                        Team = new TeamDTO { Id = p.TeamId, Name = p.Team!.Name }
                                                                    })
                                                                    .FirstOrDefaultAsync() ?? null!;

    public async Task<ServiceResult<Project>> Create(CreateProjectDTO project)
    {
        var proj = new Project
        {
            Name = project.Name,
            ProjectTypeId = project.ProjectTypeId,
            TeamId = project.TeamId
        };

        if (!_service.CanCreateModifyDeleteProjects(proj)) return ServiceResult<Project>.Unauthorized();

        _ctx.Projects.Add(proj);
        await _ctx.SaveChangesAsync();

        return ServiceResult<Project>.WithData(proj);
    }

    public async Task<ServiceResult> Update(int id, Project project)
    {
        if (id != project.Id) return ServiceResult.Badrequest();

        var entity = await _ctx.Projects.FindAsync(id);
        if (entity == null) return ServiceResult.Notfound();

        if (!_service.CanCreateModifyDeleteProjects(entity)) return ServiceResult.Unauthorized();

        entity.Name = project.Name;
        entity.ProjectTypeId = project.ProjectTypeId;

        if (_service.isAdmin())
        {
            entity.TeamId = project.TeamId;
        }

        await _ctx.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> Delete(int id)
    {
        var entity = await _ctx.Projects.FindAsync(id);
        if (entity == null) return ServiceResult.Notfound();

        if (!_service.CanCreateModifyDeleteProjects(entity)) return ServiceResult.Unauthorized();

        _ctx.Remove(entity);

        await _ctx.SaveChangesAsync();
        return ServiceResult.Success();
    }
}