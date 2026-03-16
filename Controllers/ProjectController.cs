using devhouse.Context;
using devhouse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class ProjectController : ControllerBase
{
    public DatabaseContext _ctx { get; set; }

    public ProjectController(DatabaseContext context) => _ctx = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> Get(int page = 1, int pageSize = 5)
    {
        _ = _ctx.Projects ?? throw new InvalidOperationException("Database context is malconfigured");

        return await _ctx.Projects.OrderBy(p => p.Id)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
    }

    // [HttpGet("{id}")]
    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]
}