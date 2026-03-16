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

        return await _ctx.Developers.AsNoTracking()
                                    .OrderBy(e => e.Id)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
    }

    public async Task<Developer> GetOne(int id) => await _ctx.Developers.Where(d => d.Id == id).FirstOrDefaultAsync() ?? null!;

    public async Task<Developer> Create(Developer developer)
    {
        _ctx.Developers.Add(developer);
        await _ctx.SaveChangesAsync();
        return developer;
    }

    public async Task<bool> Update(int id, Developer developer)
    {
        // To be implemented
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        // To be implemented
        return true;
    }
}