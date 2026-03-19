using devhouse.Context;
using devhouse.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using devhouse.DTOs;
namespace devhouse.Services;

public class DeveloperService
{
    public DatabaseContext _ctx { get; set; }
    public AuthService _service { get; set; }

    public DeveloperService(DatabaseContext context, AuthService service) => (_ctx, _service) = (context, service);

    public async Task<IEnumerable<ReadDeveloperDTO>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        return await _ctx.Developers.AsNoTracking()
                                    .OrderBy(e => e.Id)
                                    .Include(e => e.Team)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(d => new ReadDeveloperDTO
                                    {
                                        Id = d.Id,
                                        Firstname = d.Firstname,
                                        Lastname = d.Lastname,
                                        Email = d.Email,
                                        TeamId = d.TeamId,
                                        TeamName = d.Team!.Name,
                                        Projects = d.Team.Projects!.Select(p => p.Name).ToArray()! // Create a ReadProjectsDTO to gain some more insights
                                    })
                                    .ToListAsync();
    }

    public async Task<ReadDeveloperDTO> GetOne(int id)
        => await _ctx.Developers.AsNoTracking()
                                .Where(d => d.Id == id)
                                .Include(d => d.Team)
                                .Select(d => new ReadDeveloperDTO
                                {
                                    Id = d.Id,
                                    Firstname = d.Firstname,
                                    Lastname = d.Lastname,
                                    Email = d.Email,
                                    TeamId = d.TeamId,
                                    TeamName = d.Team!.Name,
                                    Projects = d.Team.Projects!.Select(p => p.Name).ToArray()!
                                })
                                .FirstOrDefaultAsync() ?? null!;

    public async Task<(Developer newDev, bool unauthorized)> Create(CreateDeveloperDTO developer)
    {
        var dev = new Developer
        {
            Firstname = developer.Firstname,
            Lastname = developer.Lastname,
            Email = developer.Email,
            Password = string.IsNullOrWhiteSpace(developer.Password) ? "" : HashedPassword(developer.Password),
            TeamId = developer.TeamId,
            RoleId = developer.RoleId
        };


        if (!_service.CanCreateDeleteDevelopers(dev)) return (null!, true);

        _ctx.Developers.Add(dev);

        await _ctx.SaveChangesAsync();
        return (dev, false);
    }

    public async Task<(bool notFound, bool badRequest, bool unauthorized)> Update(int id, UpdateDeveloperDTO developer)
    {
        if (id != developer.Id) return (false, true, false);

        var entity = await _ctx.Developers.FindAsync(id);
        if (entity == null) return (true, false, false);

        if (!_service.CanModifyDevelopers(entity)) return (false, false, true);

        entity.Firstname = developer.Firstname;
        entity.Lastname = developer.Lastname;
        entity.Email = developer.Email;
        entity.Password = string.IsNullOrWhiteSpace(developer.Password) ? "" : HashedPassword(developer.Password);

        if (_service.isAdmin())
        {
            entity.RoleId = developer.RoleId;
            entity.TeamId = developer.TeamId;
        }

        await _ctx.SaveChangesAsync();
        return (false, false, false);
    }

    public async Task<(bool notFound, bool unauthorized)> Delete(int id)
    {
        var developer = await _ctx.Developers.FindAsync(id);
        if (developer == null) return (true, false);

        if (!_service.CanCreateDeleteDevelopers(developer)) return (false, true);

        _ctx.Remove(developer);

        await _ctx.SaveChangesAsync();
        return (false, false);
    }

    // ! We probably want to move this out to AuthService
    // Helper method for password hashing upon creation

    // PasswordHasher doesnt really need a type, it doesnt do anything with it, its only there for safety
    // But instead of writing an interface for my DTO's, ill skip that and just pass an empty object, the hashing algorithm is what we want for now
    public string HashedPassword(string password) => new PasswordHasher<object>().HashPassword(new object(), password);


}