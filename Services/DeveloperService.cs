using devhouse.Context;
using devhouse.Models;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Services;

public class DeveloperService
{
    public DatabaseContext _ctx { get; set; }

    public DeveloperService(DatabaseContext context) => _ctx = context;

    public async Task<List<Developer>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        _ = _ctx.Developers ?? throw new InvalidOperationException("Database context is malconfigured");

        return await _ctx.Developers.AsNoTracking()
                                    .OrderBy(e => e.Id)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
    }
}