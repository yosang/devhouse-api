using devhouse.Context;
using devhouse.DTOs;
using devhouse.Models;

using Microsoft.EntityFrameworkCore;

namespace devhouse.Services;

public class TeamService
{
    public DatabaseContext _ctx { get; set; }
    public AuthService _service { get; set; }

    public TeamService(DatabaseContext context, AuthService service) => (_ctx, _service) = (context, service);

    public async Task<IEnumerable<TeamDetailsDTO>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        return await _ctx.Teams.AsNoTracking()
                                .OrderBy(t => t.Id)
                                .Include(p => p.Projects)
                                .Include(d => d.Developers)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .Select(t => new TeamDetailsDTO
                                {
                                    Id = t.Id,
                                    Name = t.Name,
                                    Projects = t.Projects!.Select(p => new ProjectDTO
                                    {
                                        Id = p.Id,
                                        Name = p.Name,
                                        ProjectType = new ProjectTypesDTO
                                        {
                                            Id = p.ProjectTypeId,
                                            Name = p.ProjectType!.Name
                                        }
                                    }),
                                    Developers = t.Developers!.Select(d => new DeveloperDTO
                                    {
                                        Id = d.Id,
                                        Firstname = d.Firstname,
                                        Lastname = d.Lastname,
                                        Role = new RoleDTO { Id = d.RoleId, Name = d.Role!.Name }
                                    })
                                })
                                .ToListAsync();
    }

    public async Task<TeamDetailsDTO?> GetOne(int id) => await _ctx.Teams.AsNoTracking()
                                                            .Where(t => t.Id == id)
                                                            .Include(p => p.Projects)
                                                            .Include(d => d.Developers)
                                                            .Select(t => new TeamDetailsDTO
                                                            {
                                                                Id = t.Id,
                                                                Name = t.Name,
                                                                Projects = t.Projects!.Select(p => new ProjectDTO
                                                                {
                                                                    Id = p.Id,
                                                                    Name = p.Name,
                                                                    ProjectType = new ProjectTypesDTO
                                                                    {
                                                                        Id = p.ProjectTypeId,
                                                                        Name = p.ProjectType!.Name
                                                                    }
                                                                }),
                                                                Developers = t.Developers!.Select(d => new DeveloperDTO
                                                                {
                                                                    Id = d.Id,
                                                                    Firstname = d.Firstname,
                                                                    Lastname = d.Lastname,
                                                                    Role = new RoleDTO { Id = d.RoleId, Name = d.Role!.Name }
                                                                })
                                                            })
                                                            .FirstOrDefaultAsync();

    public async Task<ServiceResult<Team>> Create(CreateTeamDTO dto)
    {
        var team = new Team
        {
            Name = dto.Name
        };

        if (!_service.isAdmin()) return ServiceResult<Team>.Unauthorized();

        _ctx.Teams.Add(team);
        await _ctx.SaveChangesAsync();

        return ServiceResult<Team>.WithData(team);
    }

    public async Task<ServiceResult> Update(int id, UpdateTeamDTO team)
    {
        if (id != team.Id) return ServiceResult.Badrequest();

        if (!_service.isAdmin()) return ServiceResult.Unauthorized();

        var entity = await _ctx.Teams.FindAsync(id);
        if (entity == null) return ServiceResult.Notfound();

        entity.Name = team.Name;

        await _ctx.SaveChangesAsync();
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> Delete(int id)
    {
        if (!_service.isAdmin()) return ServiceResult.Unauthorized();

        var entity = await _ctx.Teams.FindAsync(id);
        if (entity == null) return ServiceResult.Notfound();

        _ctx.Remove(entity);

        await _ctx.SaveChangesAsync();
        return ServiceResult.Success();
    }
}