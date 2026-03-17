using Microsoft.AspNetCore.Mvc;
using devhouse.Services;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    public TokenService _tokenService { get; set; }

    public AuthController(TokenService ts) => _tokenService = ts;

    [HttpPost]
    public ActionResult<string> Login()
    {
        return Ok(_tokenService.Generate("testUser"));
    }
}