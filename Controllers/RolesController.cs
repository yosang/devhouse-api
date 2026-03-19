using Microsoft.AspNetCore.Mvc;
using devhouse.Models;
using Microsoft.AspNetCore.Authorization;
using devhouse.DTOs;
using devhouse.Services;
using static devhouse.Services.ProblemFactoryService;

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
        if (role == null) return NotFound(WhenNotFound(id));
        return role;
    }

    /// <summary>Create a new role</summary>
    /// <param name="role"></param>
    /// <response code="201">Resource created</response>
    /// <response code="403">Missing required permissions</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Role), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Role>> Create(CreateRoleDTO role)
    {
        var (newRole, unauthorized) = await _service.Create(role);
        if (unauthorized) return StatusCode(StatusCodes.Status403Forbidden, WhenForbidden());

        return CreatedAtAction(nameof(Get), new { id = newRole.Id }, newRole);
    }

    /// <summary>Update a role</summary>
    /// <param name="id"></param>
    /// <param name="role"></param>
    /// <response code="204">Update successful, no content returned</response>
    /// <response code="400">Route path Id and Request body Id mismatch</response>
    /// <response code="403">Missing required permissions</response>
    /// <response code="404">Resource not found by id</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> Update(int id, Role role)
    {
        var (notFound, badRequest, unauthorized) = await _service.Update(id, role);
        if (notFound) return NotFound(WhenNotFound(id));
        if (badRequest) return BadRequest(WhenIdMismatch(id, role.Id));
        if (unauthorized) return StatusCode(StatusCodes.Status403Forbidden, WhenForbidden());

        return NoContent();
    }

    /// <summary>Delete a role</summary>
    /// <param name="id"></param>
    /// <response code="204">Deletion successful, no content returned</response>
    /// <response code="403">Missing required permissions</response>
    /// <response code="404">Resource not found by id</response>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Remove(int id)
    {
        var (notfound, unauthorized) = await _service.Delete(id);
        if (notfound) return NotFound(WhenNotFound(id));
        if (unauthorized) return StatusCode(StatusCodes.Status403Forbidden, WhenForbidden());

        return NoContent();
    }
}