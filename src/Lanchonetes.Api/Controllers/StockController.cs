using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Lanchonetes.Api.Controllers;
[ApiController]
[Route("api/stock")]
[Authorize]
public class StockController(IStockService service) : ControllerBase { [HttpPost("movements")] public async Task<IActionResult> Movement(CreateStockMovementRequest request) { await service.CreateMovementAsync(request); return StatusCode(201); } [HttpGet("{unitId:guid}/{productId:guid}")] public async Task<IActionResult> Get(Guid unitId, Guid productId) => Ok(await service.GetAsync(unitId, productId)); }
