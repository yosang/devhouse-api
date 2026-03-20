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

    /// <summary>Login for a JWT token</summary>
    /// <remarks>
    /// Sample Request:
    /// 
    ///     {
    ///         "email": "dev@dev.com",
    ///         "password": "password1234"
    ///     }
    /// 
    /// </remarks>
    /// <param name="loginDTO"></param>
    /// <returns>JWT Token on successful login</returns>
    [HttpPost("/api/[controller]/login")]
    [ProducesResponseType(typeof(TokenDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenDTO>> Login(LoginDTO loginDTO)
    {
        var result = await _service.Authenticate(loginDTO.Email, loginDTO.Password);
        if (result.invalidCredentials) return Unauthorized(ProblemResult.InvalidCredentials());

        return Ok(result.Data);
    }
}