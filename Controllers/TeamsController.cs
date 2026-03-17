using Microsoft.AspNetCore.Mvc;
using devhouse.Models;
using devhouse.Services;

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
    /// <reponse code="200">All resources retrieved</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Team>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Team>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
        => await _service.GetAll(page, pageSize);

    /// <summary> Retrieve a single team </summary>
    /// <param name="id"></param>
    /// <response code="200">Single resource retrieved</response>
    /// <response code="404">Resource not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Team), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Team>> Get(int id)
    {
        var team = await _service.GetOne(id);
        if (team == null) return NotFound();
        return team;
    }

    /// <summary>Create a new team</summary>
    /// <param name="team"></param>
    /// <response code="201">Resource created</response>
    [HttpPost]
    [ProducesResponseType(typeof(Team), StatusCodes.Status201Created)]
    public async Task<ActionResult<Team>> Create(Team team)
    {
        var newTeam = await _service.Create(team);
        return CreatedAtAction(nameof(Get), new { id = newTeam.Id }, newTeam);
    }

    /// <summary>Update a team</summary>
    /// <param name="team"></param>
    /// <response code="204">Update successful, no content returned</response>
    /// <response code="404">Resource not found by id</response>
    /// <response code="400">Route path Id and Request body Id mismatch</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, Team team)
    {
        var (notFound, badRequest) = await _service.Update(id, team);
        if (notFound) return NotFound();
        if (badRequest) return BadRequest(new ProblemDetails()
        {
            Title = "Id mismatch",
            Detail = $"There is a mismatch in the route path ({id}) Id and Request body Id ({team.Id})",
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-400-bad-request"
        });

        return NoContent();
    }

    /// <summary>Deletes a team</summary>
    /// <param name="id"></param>
    /// <response code="204">Deletion successful, no content returned</response>
    /// <response code="404">Resource not found by id</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Remove(int id)
    {
        var success = await _service.Delete(id);
        if (!success) return NotFound();

        return NoContent();
    }
}