using Microsoft.AspNetCore.Mvc;
using devhouse.Context;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class DeveloperController : ControllerBase
{
    public DatabaseContext _ctx { get; set; }

    public DeveloperController(DatabaseContext context) => _ctx = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Developer>>> Get(int page = 1, int pageSize = 5)
    {
        _ = _ctx.Developers ?? throw new InvalidOperationException("Database context is malconfigured");

        return await _ctx.Developers.OrderBy(t => t.Id)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
    }

    // [HttpGet("{id}")]
    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]
}