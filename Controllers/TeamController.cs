using Microsoft.AspNetCore.Mvc;
using devhouse.Models;
using devhouse.Services;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class TeamController : ControllerBase
{
    public TeamService _service { get; set; }

    public TeamController(TeamService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Team>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
        => await _service.GetAll(page, pageSize);

    // [HttpGet("{id}")]
    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]
}