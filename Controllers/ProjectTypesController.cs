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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectType>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
        => await _service.GetAll(page, pageSize);


    // [HttpGet("{id}")]
    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]
}