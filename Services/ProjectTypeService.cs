using devhouse.Context;
using devhouse.DTOs;
using devhouse.Models;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Services;

public class ProjectTypeService
{
    public DatabaseContext _ctx { get; set; }
    public AuthService _service { get; set; }
    public ProjectTypeService(DatabaseContext context, AuthService service) => (_ctx, _service) = (context, service);

    // ! Replace object with DTO
    public async Task<IEnumerable<object>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        return await _ctx.ProjectTypes.AsNoTracking() // No need to track all these entities as we wont mutate them here, we save some performance here
                                        .OrderBy(pt => pt.Id)
                                        .Include(pt => pt.Projects)
                                        .Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .Select(pt => new
                                        {
                                            pt.Id,
                                            pt.Name,
                                            Projects = pt.Projects!.Select(p => new
                                            {
                                                p.Id,
                                                p.Name,
                                                Team = new
                                                {
                                                    p.Team!.Id,
                                                    p.Team.Name,
                                                    Developers = p.Team.Developers!.Select(d => new { d.Id, d.Firstname, d.Lastname, })
                                                }
                                            }).ToArray()
                                        })
                                        .ToListAsync();
    }

    // ! Replace object with DTO
    public async Task<object> GetOne(int id)
        => await _ctx.ProjectTypes.AsNoTracking()
                                    .Where(pt => pt.Id == id)
                                    .Include(pt => pt.Projects)
                                    .Select(pt => new
                                    {
                                        pt.Id,
                                        pt.Name,
                                        Projects = pt.Projects!.Select(p => new
                                        {
                                            p.Id,
                                            p.Name,
                                            Team = new
                                            {
                                                p.Team!.Id,
                                                p.Team.Name,
                                                Developers = p.Team.Developers!.Select(d => new { d.Id, d.Firstname, d.Lastname, })
                                            }
                                        }).ToArray()
                                    })
                                    .FirstOrDefaultAsync() ?? null!;

    public async Task<(ProjectType project, bool unauthorized)> Create(CreateProjectTypeDTO dto)
    {
        var pt = new ProjectType
        {
            Name = dto.Name
        };

        if (!_service.isAdmin()) return (null!, true);

        _ctx.ProjectTypes.Add(pt);
        await _ctx.SaveChangesAsync();
        return (pt, false);
    }

    public async Task<(bool notFound, bool badRequest, bool unauthorized)> Update(int id, UpdateProjectTypeDTO pt)
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