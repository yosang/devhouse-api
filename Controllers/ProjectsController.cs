using devhouse.Services;
using devhouse.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class ProjectsController : ControllerBase
{
    public ProjectService _service { get; set; }

    public ProjectsController(ProjectService service) => _service = service;

    /// <summary> Retrieve a list of projects </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <reponse code="200">All resources retrieved</response>
    [HttpGet]
    [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Project>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
        => await _service.GetAll(page, pageSize);

    /// <summary> Retrieve a single project </summary>
    /// <param name="id"></param>
    /// <response code="200">Single resource retrieved</response>
    /// <response code="404">Resource not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Project>> Get(int id)
    {
        var proj = await _service.GetOne(id);
        if (proj == null) return NotFound();
        return proj;
    }

    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]
}