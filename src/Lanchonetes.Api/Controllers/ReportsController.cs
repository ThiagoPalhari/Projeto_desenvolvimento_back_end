using Lanchonetes.Application.Interfaces; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc;
namespace Lanchonetes.Api.Controllers;
[ApiController][Route("api/reports")][Authorize]
public class ReportsController(IReportService service) : ControllerBase { [HttpGet("sales")] public async Task<IActionResult> Sales(DateTime? from, DateTime? to, Guid? unitId) => Ok(await service.SalesAsync(from, to, unitId)); [HttpGet("orders")] public async Task<IActionResult> Orders(DateTime? from, DateTime? to, Guid? unitId) => Ok(await service.OrdersAsync(from, to, unitId)); [HttpGet("products")] public async Task<IActionResult> Products(DateTime? from, DateTime? to, Guid? unitId) => Ok(await service.ProductsAsync(from, to, unitId)); }
