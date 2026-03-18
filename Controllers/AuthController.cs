using Microsoft.AspNetCore.Mvc;
using devhouse.Services;
using devhouse.DTOs;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    public AuthService _service { get; set; }

    public AuthController(AuthService service) => _service = service;

    [HttpPost]
    public async Task<ActionResult<string>> Login(LoginDTO loginDTO)
    {
        var (authenticated, token) = await _service.Authenticate(loginDTO.Email, loginDTO.Password);
        if (!authenticated) return Unauthorized();

        return Ok(token);
    }
}