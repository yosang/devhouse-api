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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Developer>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
        => await _service.GetAll(page, pageSize);

    // [HttpGet("{id}")]
    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]
}