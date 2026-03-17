using Microsoft.AspNetCore.Mvc;
using devhouse.Models;
using Microsoft.AspNetCore.Authorization;

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
    /// <response code="200">All resources retrieved</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Role>), StatusCodes.Status200OK)]
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

    /// <summary>Create a new role</summary>
    /// <param name="role"></param>
    /// <response code="201">Resource created</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Role), StatusCodes.Status201Created)]
    public async Task<ActionResult<Role>> Create(Role role)
    {
        var newRole = await _service.Create(role);
        return CreatedAtAction(nameof(Get), new { id = newRole.Id }, newRole);
    }

    /// <summary>Update a role</summary>
    /// <param name="id"></param>
    /// <param name="role"></param>
    /// <response code="204">Update successful, no content returned</response>
    /// <response code="404">Resource not found by id</response>
    /// <response code="400">Route path Id and Request body Id mismatch</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> Update(int id, Role role)
    {
        var (notFound, badRequest) = await _service.Update(id, role);
        if (notFound) return NotFound();
        if (badRequest) return BadRequest(new ProblemDetails()
        {
            Title = "Id mismatch",
            Detail = $"There is a mismatch in the route path ({id}) Id and Request body Id ({role.Id})",
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-400-bad-request"
        });

        return NoContent();
    }

    /// <summary>Delete a role</summary>
    /// <param name="id"></param>
    /// <response code="204">Deletion successful, no content returned</response>
    /// <response code="404">Resource not found by id</response>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Remove(int id)
    {
        var success = await _service.Delete(id);
        if (!success) return NotFound();

        return NoContent();
    }
}