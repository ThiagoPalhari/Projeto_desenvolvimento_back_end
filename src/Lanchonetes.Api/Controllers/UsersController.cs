using Lanchonetes.Application.DTOs.Requests; using Lanchonetes.Application.Interfaces; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc;
namespace Lanchonetes.Api.Controllers;
[ApiController][Route("api/users")][Authorize]
public class UsersController(IUserService service) : ControllerBase { [HttpGet] public async Task<IActionResult> GetAll() => Ok(await service.GetAllAsync()); [HttpGet("{id:guid}")] public async Task<IActionResult> Get(Guid id) => Ok(await service.GetAsync(id)); [HttpPut("{id:guid}")] public async Task<IActionResult> Update(Guid id, UpdateUserRequest request) => Ok(await service.UpdateAsync(id, request)); [HttpDelete("{id:guid}")] public async Task<IActionResult> Delete(Guid id) { await service.DeleteAsync(id); return NoContent(); } }
