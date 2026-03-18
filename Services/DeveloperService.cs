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

    public async Task<List<Developer>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        return await _ctx.Developers.AsNoTracking()
                                    .OrderBy(e => e.Id)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
    }

    public async Task<Developer> GetOne(int id) => await _ctx.Developers.Where(d => d.Id == id).FirstOrDefaultAsync() ?? null!;

    public async Task<(Developer newDev, bool unauthorized)> Create(Developer developer)
    {
        if (!_service.isAuthorized(developer)) return (null!, true);

        _ctx.Developers.Add(developer);

        developer.Password = HashedPassword(developer);

        await _ctx.SaveChangesAsync();
        return (developer, false);
    }

    public async Task<(bool notFound, bool badRequest, bool unauthorized)> Update(int id, Developer developer)
    {
        if (id != developer.Id) return (false, true, false);

        var entity = await _ctx.Developers.FindAsync(id);
        if (entity == null) return (true, false, false);

        if (!_service.isAuthorized(entity)) return (false, false, true);

        entity.Firstname = developer.Firstname;
        entity.Lastname = developer.Lastname;
        entity.Email = developer.Email;
        entity.Password = HashedPassword(developer);

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

        if (!_service.isAuthorized(developer)) return (false, true);

        _ctx.Remove(developer);

        await _ctx.SaveChangesAsync();
        return (false, false);
    }

    // ! We probably want to move this out to AuthService

    // Helper method for password hashing upon creation
    public string HashedPassword(Developer developer) => new PasswordHasher<Developer>().HashPassword(developer, developer.Password!);


}