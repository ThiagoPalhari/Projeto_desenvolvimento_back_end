using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lanchonetes.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IUserService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await service.RegisterAsync(request, cancellationToken));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            return Ok(await service.LoginAsync(request));
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(new { message = exception.Message });
        }
    }
}