using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lanchonetes.Api.Controllers;

[ApiController]
[Route("api/products")]
[Authorize]
public class ProductsController(IProductService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await service.GetAllAsync());

    [HttpGet("menu/{unitId:guid}")]
    public async Task<IActionResult> GetMenu(Guid unitId)
        => Ok(await service.GetMenuAsync(unitId));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
        => Ok(await service.GetAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        await service.CreateAsync(request);
        return StatusCode(201);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateProductRequest request)
    {
        await service.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{productId:guid}/units/{unitId:guid}/availability")]
    public async Task<IActionResult> Availability(
        Guid productId,
        Guid unitId,
        SetProductAvailabilityRequest request)
    {
        await service.SetAvailabilityAsync(
            unitId,
            productId,
            request);

        return NoContent();
    }
}
