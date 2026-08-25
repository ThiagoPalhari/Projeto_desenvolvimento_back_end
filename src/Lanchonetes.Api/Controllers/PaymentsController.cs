using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Lanchonetes.Api.Controllers;
[ApiController]
[Route("api/payments")]
[Authorize]
public class PaymentsController(IPaymentService service) : ControllerBase { [HttpPost("process")] public async Task<IActionResult> Process(ProcessPaymentRequest request) => Ok(await service.ProcessAsync(request)); [AllowAnonymous][HttpPost("callback")] public async Task<IActionResult> Callback(PaymentCallbackRequest request) => Ok(await service.CallbackAsync(request)); [HttpGet("{id:guid}")] public async Task<IActionResult> Get(Guid id) => Ok(await service.GetAsync(id)); }
