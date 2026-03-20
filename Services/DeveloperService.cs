using devhouse.Context;
using devhouse.Models;
using Microsoft.EntityFrameworkCore;
using devhouse.DTOs;

namespace devhouse.Services;

public class DeveloperService
{
    public DatabaseContext _ctx { get; set; }
    public AuthService _service { get; set; }

    public DeveloperService(DatabaseContext context, AuthService service) => (_ctx, _service) = (context, service);

    public async Task<IEnumerable<DeveloperDetailsDTO>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        return await _ctx.Developers.AsNoTracking()
                                    .OrderBy(e => e.Id)
                                    .Include(e => e.Team)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(d => new DeveloperDetailsDTO
                                    {
                                        Id = d.Id,
                                        Firstname = d.Firstname,
                                        Lastname = d.Lastname,
                                        Role = new RoleDTO { Id = d.RoleId, Name = d.Role!.Name },
                                        Team = new TeamDTO { Id = d.TeamId, Name = d.Team!.Name },
                                        Projects = d.Team.Projects!.Select(p => new ProjectDTO
                                        {
                                            Id = p.Id,
                                            Name = p.Name,
                                            ProjectType = new ProjectTypesDTO { Id = p.ProjectTypeId, Name = p.ProjectType!.Name }
                                        })
                                    })
                                    .ToListAsync();
    }

    public async Task<DeveloperDetailsDTO?> GetOne(int id)
        => await _ctx.Developers.AsNoTracking()
                                .Where(d => d.Id == id)
                                .Include(d => d.Team)
                                .Select(d => new DeveloperDetailsDTO
                                {
                                    Id = d.Id,
                                    Firstname = d.Firstname,
                                    Lastname = d.Lastname,
                                    Role = new RoleDTO { Id = d.RoleId, Name = d.Role!.Name },
                                    Team = new TeamDTO { Id = d.TeamId, Name = d.Team!.Name },
                                    Projects = d.Team.Projects!.Select(p => new ProjectDTO
                                    {
                                        Id = p.Id,
                                        Name = p.Name,
                                        ProjectType = new ProjectTypesDTO { Id = p.ProjectTypeId, Name = p.ProjectType!.Name }
                                    })
                                })
                                .FirstOrDefaultAsync();

    public async Task<ServiceResult<Developer>> Create(CreateDeveloperDTO developer)
    {
        var dev = new Developer
        {
            Firstname = developer.Firstname,
            Lastname = developer.Lastname,
            Email = developer.Email,
            Password = string.IsNullOrWhiteSpace(developer.Password) ? developer.Password : _service.HashedPassword(developer.Password),
            TeamId = developer.TeamId,
            RoleId = developer.RoleId
        };


        if (!_service.CanCreateDeleteDevelopers(dev)) return ServiceResult<Developer>.Unauthorized();

        _ctx.Developers.Add(dev);

        await _ctx.SaveChangesAsync();
        return ServiceResult<Developer>.WithData(dev);
    }

    public async Task<ServiceResult> Update(int id, UpdateDeveloperDTO developer)
    {
        if (id != developer.Id) return ServiceResult<Developer>.Badrequest();

        var entity = await _ctx.Developers.FindAsync(id);
        if (entity == null) return ServiceResult<Developer>.Notfound();

        if (!_service.CanModifyDevelopers(entity)) return ServiceResult<Developer>.Unauthorized();

        entity.Firstname = developer.Firstname;
        entity.Lastname = developer.Lastname;
        entity.Email = developer.Email;
        entity.Password = string.IsNullOrWhiteSpace(developer.Password) ? developer.Password : _service.HashedPassword(developer.Password);

        if (_service.isAdmin())
        {
            entity.RoleId = developer.RoleId;
            entity.TeamId = developer.TeamId;
        }

        await _ctx.SaveChangesAsync();
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> Delete(int id)
    {
        var developer = await _ctx.Developers.FindAsync(id);
        if (developer == null) return ServiceResult.Notfound();

        if (!_service.CanCreateDeleteDevelopers(developer)) return ServiceResult.Unauthorized();

        _ctx.Remove(developer);

        await _ctx.SaveChangesAsync();
        return ServiceResult.Success();
    }

}