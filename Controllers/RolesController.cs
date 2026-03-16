using Microsoft.AspNetCore.Mvc;
using devhouse.Models;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class RolesController : ControllerBase
{
    public RoleService _service { get; set; }

    public RolesController(RoleService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
            => await _service.GetAll(page, pageSize);

    // [HttpGet("{id}")]
    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]
}