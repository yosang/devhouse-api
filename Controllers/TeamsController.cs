using Microsoft.AspNetCore.Mvc;
using devhouse.Models;
using devhouse.Services;
using Microsoft.AspNetCore.Authorization;
using devhouse.DTOs;
using static devhouse.Services.ProblemFactoryService;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class TeamsController : ControllerBase
{
    public TeamService _service { get; set; }

    public TeamsController(TeamService service) => _service = service;

    /// <summary> Retrieve a list of teams </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <response code="200">All resources retrieved</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReadTeamDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReadTeamDTO>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
        => Ok(await _service.GetAll(page, pageSize));

    /// <summary> Retrieve a single team </summary>
    /// <param name="id"></param>
    /// <response code="200">Single resource retrieved</response>
    /// <response code="404">Resource not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Team), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadTeamDTO>> Get(int id)
    {
        var team = await _service.GetOne(id);
        if (team == null) return NotFound(WhenNotFound(id));
        return team;
    }

    /// <summary>Create a new team</summary>
    /// <param name="team"></param>
    /// <response code="201">Resource created</response>
    /// <response code="403">Missing required permissions</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Team), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Team), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Team>> Create(CreateTeamDTO team)
    {
        var result = await _service.Create(team);
        if (result.unauthorized) return StatusCode(StatusCodes.Status403Forbidden, WhenForbidden());

        return CreatedAtAction(nameof(Get), new { id = result.Data!.Id }, result);
    }

    /// <summary>Update a team</summary>
    /// <param name="id"></param>
    /// <param name="team"></param>
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
    public async Task<ActionResult> Update(int id, UpdateTeamDTO team)
    {
        var result = await _service.Update(id, team);
        if (result.notFound) return NotFound(WhenNotFound(id));
        if (result.badRequest) return BadRequest(WhenIdMismatch(id, team.Id));
        if (result.unauthorized) return StatusCode(StatusCodes.Status403Forbidden, WhenForbidden());

        return NoContent();
    }

    /// <summary>Delete a team</summary>
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
        var result = await _service.Delete(id);
        if (result.notFound) return NotFound(WhenNotFound(id));
        if (result.unauthorized) return StatusCode(StatusCodes.Status403Forbidden, WhenForbidden());

        return NoContent();
    }
}