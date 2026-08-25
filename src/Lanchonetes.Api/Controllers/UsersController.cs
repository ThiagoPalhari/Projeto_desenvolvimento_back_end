using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lanchonetes.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await userService.RegisterAsync(request, cancellationToken);

            return Created($"/api/users/{user.Id}", user);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }
}