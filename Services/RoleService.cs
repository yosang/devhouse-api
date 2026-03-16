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
        _ = _ctx.Roles ?? throw new InvalidOperationException("Database context is malconfigured");

        return await _ctx.Roles.AsNoTracking()
                                .OrderBy(r => r.Id)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
    }
}