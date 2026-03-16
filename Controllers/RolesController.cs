using Microsoft.AspNetCore.Mvc;
using devhouse.Models;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class RolesController : ControllerBase
{
    public RoleService _service { get; set; }

    public RolesController(RoleService service) => _service = service;

    /// <summary> Retrieve a list of roles </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <reponse code="200">All resources retrieved</response>
    [HttpGet]
    [ProducesResponseType(typeof(Role), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Role>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
            => await _service.GetAll(page, pageSize);

    /// <summary> Retrieve a single role </summary>
    /// <param name="id"></param>
    /// <response code="200">Single resource retrieved</response>
    /// <response code="404">Resource not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Role), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Role>> Get(int id)
    {
        var role = await _service.GetOne(id);
        if (role == null) return NotFound();
        return role;
    }

    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]
}