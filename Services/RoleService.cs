using devhouse.Context;
using devhouse.DTOs;
using devhouse.Models;
using devhouse.Services;
using Microsoft.EntityFrameworkCore;

public class RoleService
{
    public DatabaseContext _ctx { get; set; }
    public AuthService _service { get; set; }

    public RoleService(DatabaseContext context, AuthService service) => (_ctx, _service) = (context, service);

    public async Task<List<Role>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        return await _ctx.Roles.AsNoTracking()
                                .OrderBy(r => r.Id)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
    }

    public async Task<Role> GetOne(int id) => await _ctx.Roles.Where(r => r.Id == id).FirstOrDefaultAsync() ?? null!;

    public async Task<(Role role, bool unauthorized)> Create(CreateRoleDTO dto)
    {
        var role = new Role
        {
            Name = dto.Name
        };

        if (!_service.isAdmin()) return (null!, true);

        _ctx.Roles.Add(role);
        await _ctx.SaveChangesAsync();

        return (role, false);
    }

    public async Task<(bool notFound, bool badRequest, bool unauthorized)> Update(int id, Role role)
    {
        if (id != role.Id) return (false, true, false);

        if (!_service.isAdmin()) return (false, false, true);

        var entity = await _ctx.Roles.FindAsync(id);
        if (entity == null) return (true, false, false);

        entity.Name = role.Name;

        await _ctx.SaveChangesAsync();
        return (false, false, false);
    }

    public async Task<(bool notFound, bool unauthorized)> Delete(int id)
    {
        if (!_service.isAdmin()) return (false, true);

        var entity = await _ctx.Roles.FindAsync(id);
        if (entity == null) return (true, false);

        _ctx.Remove(entity);

        await _ctx.SaveChangesAsync();
        return (false, false);
    }
}