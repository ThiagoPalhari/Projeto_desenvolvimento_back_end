using Lanchonetes.Application.Interfaces; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc;
namespace Lanchonetes.Api.Controllers;
[ApiController][Route("api/audit")][Authorize]
public class AuditController(IAuditService service) : ControllerBase { [HttpGet] public async Task<IActionResult> Get([FromQuery] DateTime? from, [FromQuery] DateTime? to) => Ok(await service.GetAsync(from, to)); }
