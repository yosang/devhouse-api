using devhouse.Context;
using devhouse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class ProjectTypeController : ControllerBase
{

    public DatabaseContext _ctx { get; set; }

    public ProjectTypeController(DatabaseContext context)
    {
        _ctx = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectType>>> Get(int page = 1, int pageSize = 5)
    {
        _ = _ctx.ProjectTypes ?? throw new InvalidOperationException("Entity or databsae context is malconfigured");

        return await _ctx.ProjectTypes.OrderBy(pt => pt.Id)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
    }
    // [HttpGet("{id}")]
    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]
}