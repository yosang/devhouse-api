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
    /// <response code="200">All resources retrieved</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Project>), StatusCodes.Status200OK)]
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

    /// <summary>Create a new project</summary>
    /// <param name="project"></param>
    /// <response code="201">Resource created</response>
    [HttpPost]
    [ProducesResponseType(typeof(Project), StatusCodes.Status201Created)]
    public async Task<ActionResult<Project>> Create(Project project)
    {
        var newProject = await _service.Create(project);
        return CreatedAtAction(nameof(Get), new { id = newProject.Id }, newProject);
    }

    /// <summary>Update a project</summary>
    /// <param name="id"></param>
    /// <param name="project"></param>
    /// <response code="204">Update successful, no content returned</response>
    /// <response code="404">Resource not found by id</response>
    /// <response code="400">Route path Id and Request body Id mismatch</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, Project project)
    {
        var (notFound, badRequest) = await _service.Update(id, project);
        if (notFound) return NotFound();
        if (badRequest) return BadRequest(new ProblemDetails()
        {
            Title = "Id mismatch",
            Detail = $"There is a mismatch in the route path ({id}) Id and Request body Id ({project.Id})",
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-400-bad-request"
        });

        return NoContent();
    }

    /// <summary>Delete a project</summary>
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