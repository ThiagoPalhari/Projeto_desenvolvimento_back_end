using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Lanchonetes.Api.Controllers;
[ApiController]
[Route("api/promotions")]
[Authorize]
public class PromotionsController(IPromotionService service) : ControllerBase { [HttpGet] public async Task<IActionResult> GetAll() => Ok(await service.GetAllAsync()); [HttpPost] public async Task<IActionResult> Create(CreatePromotionRequest request) { await service.CreateAsync(request); return StatusCode(201); } [HttpPut("{id:guid}")] public async Task<IActionResult> Update(Guid id, UpdatePromotionRequest request) { await service.UpdateAsync(id, request); return NoContent(); } [HttpDelete("{id:guid}")] public async Task<IActionResult> Delete(Guid id) { await service.DeleteAsync(id); return NoContent(); } [HttpPost("calculate")] public async Task<IActionResult> Calculate(Guid customerId, Guid orderId) => Ok(await service.CalculateAsync(customerId, orderId)); }
