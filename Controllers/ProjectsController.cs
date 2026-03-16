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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
        => await _service.GetAll(page, pageSize);

    // [HttpGet("{id}")]
    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]
}