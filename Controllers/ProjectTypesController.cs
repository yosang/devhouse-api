using devhouse.Services;
using devhouse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using devhouse.DTOs;

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
    /// <response code="200">All resources retrieved</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProjectType>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<object>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
        => Ok(await _service.GetAll(page, pageSize));


    /// <summary> Retrieve a single project type </summary>
    /// <param name="id"></param>
    /// <response code="200">Single resource retrieved</response>
    /// <response code="404">Resource not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProjectType), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<object>> Get(int id)
    {
        var ptype = await _service.GetOne(id);
        if (ptype == null) return NotFound();
        return ptype;
    }

    /// <summary>Create a new project type</summary>
    /// <param name="pt"></param>
    /// <response code="201">Resource created</response>
    /// <response code="403">Missing required permissions</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ProjectType), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ProjectType>> Create(CreateProjectTypeDTO pt)
    {
        var (newPt, unauthorized) = await _service.Create(pt);
        if (unauthorized) return StatusCode(StatusCodes.Status403Forbidden, ProblemFactoryService.Forbidden());

        return CreatedAtAction(nameof(Get), new { id = newPt.Id }, newPt);
    }

    /// <summary>Update a project type</summary>
    /// <param name="id"></param>
    /// <param name="projecttype"></param>
    /// <response code="204">Update successful, no content returned</response>
    /// <response code="403">Missing required permissions</response>
    /// <response code="404">Resource not found by id</response>
    /// <response code="400">Route path Id and Request body Id mismatch</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> Update(int id, UpdateProjectTypeDTO projecttype)
    {
        var (notFound, badRequest, unauthorized) = await _service.Update(id, projecttype);

        if (notFound) return NotFound(ProblemFactoryService.NotFound(id));
        if (badRequest) return BadRequest(ProblemFactoryService.BadRequestIdMismatch(id, projecttype.Id));
        if (unauthorized) return StatusCode(StatusCodes.Status403Forbidden, ProblemFactoryService.Forbidden());

        return NoContent();
    }

    /// <summary>Delete a project type</summary>
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
        var (notFound, unauthorized) = await _service.Delete(id);
        if (notFound) return NotFound();
        if (unauthorized) return Forbid();

        return NoContent();
    }
}