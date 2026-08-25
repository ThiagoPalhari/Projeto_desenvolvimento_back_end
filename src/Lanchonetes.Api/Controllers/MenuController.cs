using Lanchonetes.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace Lanchonetes.Api.Controllers;
[ApiController]
[Route("api/units/{unitId:guid}/menu")]
public class MenuController(IProductService service) : ControllerBase { [HttpGet] public async Task<IActionResult> Get(Guid unitId) => Ok(await service.GetMenuAsync(unitId)); }
