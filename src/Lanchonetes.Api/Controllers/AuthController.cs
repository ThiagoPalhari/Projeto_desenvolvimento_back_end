using Lanchonetes.Application.DTOs.Requests; using Lanchonetes.Application.Interfaces; using Microsoft.AspNetCore.Mvc;
namespace Lanchonetes.Api.Controllers;
[ApiController][Route("api/auth")]
public class AuthController(IUserService service) : ControllerBase { [HttpPost("register")] public async Task<IActionResult> Register(RegisterUserRequest request) => Ok(await service.RegisterAsync(request)); [HttpPost("login")] public async Task<IActionResult> Login(LoginRequest request) => Ok(await service.LoginAsync(request)); }
