using Microsoft.AspNetCore.Mvc;
using devhouse.Services;
using devhouse.Models;


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
    /// <reponse code="200">All resources retrieved</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Developer>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Developer>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
        => await _service.GetAll(page, pageSize);

    /// <summary> Retrieve a single developer </summary>
    /// <param name="id"></param>
    /// <response code="200">Single resource retrieved</response>
    /// <response code="404">Resource not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Developer), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Developer>> Get(int id)
    {
        var dev = await _service.GetOne(id);
        if (dev == null) return NotFound();
        return dev;
    }


    /// <summary>Create a new developer</summary>
    /// <param name="developer"></param>
    /// <response code="201">Resource created</response>
    [HttpPost]
    [ProducesResponseType(typeof(Developer), StatusCodes.Status201Created)]
    public async Task<ActionResult<Developer>> Create(Developer developer)
    {
        var newDev = await _service.Create(developer);
        return CreatedAtAction(nameof(Get), new { id = newDev.Id }, newDev);
    }

    /// <summary>Update a developer</summary>
    /// <param name="developer"></param>
    /// <response code="204">Update successful, no content returned</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPut("{id}")]
    public async Task<ActionResult<Developer>> Update(int id, Developer developer)
    {
        var success = await _service.Update(id, developer);
        if (!success) return NotFound();

        return NoContent();
    }

    // [HttpDelete]
}