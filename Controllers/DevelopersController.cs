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
    [ProducesResponseType(typeof(Developer), StatusCodes.Status200OK)]
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

    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]
}