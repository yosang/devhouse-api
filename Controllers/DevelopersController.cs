using Microsoft.AspNetCore.Mvc;
using devhouse.Services;
using devhouse.Models;
using Microsoft.AspNetCore.Authorization;
using devhouse.DTOs;
using Mysqlx.Crud;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class DevelopersController : ControllerBase
{
    public DeveloperService _service { get; set; }

    public DevelopersController(DeveloperService service) => _service = service;

    /// <summary> Retrieve a list of developers </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <response code="200">All resources retrieved</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReadDeveloperDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReadDeveloperDTO>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
        => Ok(await _service.GetAll(page, pageSize)); // When we are not returning an ASP.NET Core entity like Developer, but a DTO, we must explicitly wrap it

    /// <summary> Retrieve a single developer </summary>
    /// <param name="id"></param>
    /// <response code="200">Single resource retrieved</response>
    /// <response code="404">Resource not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReadDeveloperDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadDeveloperDTO>> Get(int id)
    {
        var dev = await _service.GetOne(id);
        if (dev == null) return NotFound(ProblemFactoryService.NotFound(id));
        return dev;
    }


    /// <summary>Create a new developer</summary>
    /// <param name="developer"></param>
    /// <response code="201">Resource created</response>
    /// <response code="403">Missing required permissions</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Developer), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Developer>> Create(CreateDeveloperDTO developer)
    {
        var (newDev, unauthorized) = await _service.Create(developer);
        if (unauthorized) return StatusCode(StatusCodes.Status403Forbidden, ProblemFactoryService.Forbidden());

        return CreatedAtAction(nameof(Get), new { id = newDev.Id }, newDev);
    }

    /// <summary>Update a developer</summary>
    /// <param name="id"></param>
    /// <param name="developer"></param>
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
    public async Task<ActionResult> Update(int id, UpdateDeveloperDTO developer)
    {
        var (notFound, badRequest, unauthorized) = await _service.Update(id, developer);
        if (notFound) return NotFound(ProblemFactoryService.NotFound(developer.Id));
        if (badRequest) return BadRequest(ProblemFactoryService.BadRequestIdMismatch(id, developer.Id));
        if (unauthorized) return StatusCode(StatusCodes.Status403Forbidden, ProblemFactoryService.Forbidden());

        return NoContent();
    }

    /// <summary>Delete a developer</summary>
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
        if (notFound) return NotFound(ProblemFactoryService.NotFound(id));
        if (unauthorized) return StatusCode(StatusCodes.Status403Forbidden, ProblemFactoryService.Forbidden());

        return NoContent();
    }
}