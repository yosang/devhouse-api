using devhouse.Context;
using devhouse.DTOs;
using devhouse.Models;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Services;

public class RoleService
{

    /// <summary>DbContext</summary>
    public DatabaseContext _ctx { get; set; }

    /// <summary>AuthService</summary>    
    public AuthService _service { get; set; }

    // Injects DbContext and AuthService
    public RoleService(DatabaseContext context, AuthService service) => (_ctx, _service) = (context, service);

    public async Task<List<RoleDTO>> GetAll(int page = 1, int pageSize = 5)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

        return await _ctx.Roles.AsNoTracking()
                                .OrderBy(r => r.Id)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .Select(r => new RoleDTO { Id = r.Id, Name = r.Name })
                                .ToListAsync();
    }

    public async Task<RoleDTO?> GetOne(int id) => await _ctx.Roles.AsNoTracking()
                                                                .Where(r => r.Id == id)
                                                                .Select(r => new RoleDTO { Id = r.Id, Name = r.Name })
                                                                .FirstOrDefaultAsync();

    public async Task<ServiceResult<Role>> Create(CreateRoleDTO dto)
    {
        var role = new Role
        {
            Name = dto.Name
        };

        if (!_service.isAdmin()) return ServiceResult<Role>.Unauthorized();

        _ctx.Roles.Add(role);
        await _ctx.SaveChangesAsync();

        return ServiceResult<Role>.WithData(role);
    }

    public async Task<ServiceResult> Update(int id, UpdateRoleDTO role)
    {
        if (id != role.Id) return ServiceResult.Badrequest();

        if (!_service.isAdmin()) return ServiceResult.Unauthorized();

        var entity = await _ctx.Roles.FindAsync(id);
        if (entity == null) return ServiceResult.Notfound();

        entity.Name = role.Name;

        await _ctx.SaveChangesAsync();
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> Delete(int id)
    {
        if (!_service.isAdmin()) return ServiceResult.Unauthorized();

        var entity = await _ctx.Roles.FindAsync(id);
        if (entity == null) return ServiceResult.Notfound();

        _ctx.Remove(entity);

        await _ctx.SaveChangesAsync();
        return ServiceResult.Success();
    }
}