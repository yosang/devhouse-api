using devhouse.Context;
using devhouse.DTOs;
using devhouse.Models;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Services;

public class ProjectTypeService
{
    public DatabaseContext _ctx { get; set; }
    public AuthService _service { get; set; }

    // Injects DbContext and AuthService
    public ProjectTypeService(DatabaseContext context, AuthService service) => (_ctx, _service) = (context, service);

    public async Task<IEnumerable<ProjectTypesDetailsDTO>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        return await _ctx.ProjectTypes.AsNoTracking() // No need to track all these entities as we wont mutate them here, we save some performance here
                                        .OrderBy(pt => pt.Id)
                                        .Include(pt => pt.Projects)
                                        .Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .Select(pt => new ProjectTypesDetailsDTO
                                        {
                                            Id = pt.Id,
                                            Name = pt.Name,
                                            Projects = pt.Projects!.Select(p => new ProjectForProjectTypesDTO
                                            {
                                                Id = p.Id,
                                                Name = p.Name,
                                                Team = new TeamDTO
                                                {
                                                    Id = p.Team!.Id,
                                                    Name = p.Team.Name,
                                                    Developers = p.Team.Developers!.Select(d => new DeveloperDTO
                                                    {
                                                        Id =
                                                        d.Id,
                                                        Firstname =
                                                        d.Firstname,
                                                        Lastname = d.Lastname,
                                                        Role = new RoleDTO { Id = d.RoleId, Name = d.Role!.Name }
                                                    })
                                                }
                                            }).ToArray()
                                        })
                                        .ToListAsync();
    }

    // ! Replace object with DTO
    public async Task<ProjectTypesDetailsDTO?> GetOne(int id)
        => await _ctx.ProjectTypes.AsNoTracking()
                                    .Where(pt => pt.Id == id)
                                    .Include(pt => pt.Projects)
                                     .Select(pt => new ProjectTypesDetailsDTO
                                     {
                                         Id = pt.Id,
                                         Name = pt.Name,
                                         Projects = pt.Projects!.Select(p => new ProjectForProjectTypesDTO
                                         {
                                             Id = p.Id,
                                             Name = p.Name,
                                             Team = new TeamDTO
                                             {
                                                 Id = p.Team!.Id,
                                                 Name = p.Team.Name,
                                                 Developers = p.Team.Developers!.Select(d => new DeveloperDTO
                                                 {
                                                     Id = d.Id,
                                                     Firstname = d.Firstname,
                                                     Lastname = d.Lastname,
                                                     Role = new RoleDTO { Id = d.RoleId, Name = d.Role!.Name }
                                                 })
                                             }
                                         }).ToArray()
                                     })
                                    .FirstOrDefaultAsync();

    public async Task<ServiceResult<ProjectType>> Create(CreateProjectTypeDTO dto)
    {
        var pt = new ProjectType
        {
            Name = dto.Name
        };

        if (!_service.isAdmin()) return ServiceResult<ProjectType>.Unauthorized();

        _ctx.ProjectTypes.Add(pt);
        await _ctx.SaveChangesAsync();
        return ServiceResult<ProjectType>.WithData(pt);
    }

    public async Task<ServiceResult> Update(int id, UpdateProjectTypeDTO pt)
    {
        if (id != pt.Id) return ServiceResult.Badrequest();

        if (!_service.isAdmin()) return ServiceResult.Unauthorized();

        var entity = await _ctx.ProjectTypes.FindAsync(id);
        if (entity == null) return ServiceResult.Notfound();

        entity.Name = pt.Name;

        await _ctx.SaveChangesAsync();
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> Delete(int id)
    {
        if (!_service.isAdmin()) return ServiceResult.Unauthorized();

        var entity = await _ctx.ProjectTypes.FindAsync(id);
        if (entity == null) return ServiceResult.Notfound();

        _ctx.Remove(entity);

        await _ctx.SaveChangesAsync();
        return ServiceResult.Success();
    }
}