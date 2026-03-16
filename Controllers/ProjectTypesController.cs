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
    [ProducesResponseType(typeof(ProjectType), StatusCodes.Status200OK)]
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

    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]
}