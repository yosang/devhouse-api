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
    [ProducesResponseType(typeof(Team), StatusCodes.Status200OK)]
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

    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]
}