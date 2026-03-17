using devhouse.Services;
using devhouse.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class ProjectTypesController : ControllerBase
{
    public ProjectTypeService _service { get; set; }

    public ProjectTypesController(ProjectTypeService service) => _service = service;

    /// <summary> Retrieve a list of project types </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <reponse code="200">All resources retrieved</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProjectType>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProjectType>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
        => await _service.GetAll(page, pageSize);


    /// <summary> Retrieve a single project type </summary>
    /// <param name="id"></param>
    /// <response code="200">Single resource retrieved</response>
    /// <response code="404">Resource not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProjectType), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectType>> Get(int id)
    {
        var ptype = await _service.GetOne(id);
        if (ptype == null) return NotFound();
        return ptype;
    }

    /// <summary>Create a new project type</summary>
    /// <param name="pt"></param>
    /// <response code="201">Resource created</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProjectType), StatusCodes.Status201Created)]
    public async Task<ActionResult<ProjectType>> Create(ProjectType pt)
    {
        var newPt = await _service.Create(pt);
        return CreatedAtAction(nameof(Get), new { id = newPt.Id }, newPt);
    }

    /// <summary>Update a project type</summary>
    /// <param name="projecttype"></param>
    /// <response code="204">Update successful, no content returned</response>
    /// <response code="404">Resource not found by id</response>
    /// <response code="400">Route path Id and Request body Id mismatch</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, ProjectType projecttype)
    {
        var (notFound, badRequest) = await _service.Update(id, projecttype);
        if (notFound) return NotFound();
        if (badRequest) return BadRequest(new ProblemDetails()
        {
            Title = "Id mismatch",
            Detail = $"There is a mismatch in the route path ({id}) Id and Request body Id ({projecttype.Id})",
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-400-bad-request"
        });

        return NoContent();
    }
    // [HttpDelete]
}