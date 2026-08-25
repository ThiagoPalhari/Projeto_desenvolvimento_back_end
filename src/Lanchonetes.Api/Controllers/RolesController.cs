using Lanchonetes.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Lanchonetes.Api.Controllers;
[ApiController]
[Route("api/roles")]
[Authorize]
public class RolesController(IRoleService service) : ControllerBase { [HttpPost] public async Task<IActionResult> Create([FromBody] string name) { await service.CreateAsync(name); return NoContent(); } [HttpPost("{roleId:guid}/permissions/{permissionId:guid}")] public async Task<IActionResult> Assign(Guid roleId, Guid permissionId) { await service.AssignPermissionAsync(roleId, permissionId); return NoContent(); } [HttpGet("{roleId:guid}/permissions")] public async Task<IActionResult> Permissions(Guid roleId) => Ok(await service.GetPermissionsAsync(roleId)); }
