using devhouse.Context;
using devhouse.Models;
using Microsoft.EntityFrameworkCore;

public class RoleService
{
    public DatabaseContext _ctx { get; set; }

    public RoleService(DatabaseContext context) => _ctx = context;

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

    public async Task<Role> Create(Role role)
    {
        _ctx.Roles.Add(role);
        await _ctx.SaveChangesAsync();
        return role;
    }

    public async Task<(bool notFound, bool badRequest)> Update(int id, Role role)
    {
        if (id != role.Id) return (false, true);

        var entity = await _ctx.Roles.FindAsync(id);
        if (entity == null) return (true, false);

        entity.Name = role.Name;

        await _ctx.SaveChangesAsync();
        return (false, false);
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await _ctx.Roles.FindAsync(id);
        if (entity == null) return false;

        _ctx.Remove(entity);

        await _ctx.SaveChangesAsync();
        return true;
    }
}