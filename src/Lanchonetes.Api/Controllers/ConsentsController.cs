using Lanchonetes.Application.DTOs.Requests; using Lanchonetes.Application.Interfaces; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc;
namespace Lanchonetes.Api.Controllers;
[ApiController][Route("api/consents")][Authorize]
public class ConsentsController(IConsentService service) : ControllerBase { [HttpPost] public async Task<IActionResult> Create(CreateConsentRequest request) { await service.CreateAsync(request); return StatusCode(201); } [HttpGet("customer/{customerId:guid}")] public async Task<IActionResult> Get(Guid customerId) => Ok(await service.GetByCustomerAsync(customerId)); [HttpPost("{id:guid}/revoke")] public async Task<IActionResult> Revoke(Guid id, RevokeConsentRequest request) { await service.RevokeAsync(id, request); return NoContent(); } }
