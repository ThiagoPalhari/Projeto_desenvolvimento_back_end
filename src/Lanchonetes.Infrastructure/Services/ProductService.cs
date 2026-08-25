using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.Interfaces;
using Lanchonetes.Domain.Entities;
using Lanchonetes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lanchonetes.Infrastructure.Services;

public class ProductService(AppDbContext context) : IProductService
{
    public async Task CreateAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Active = true
        };

        context.Products.Add(product);

        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateProductRequest request)
    {
        var product = await context.Products
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            throw new KeyNotFoundException("Produto não encontrado.");

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Active = request.Active;

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await context.Products
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            throw new KeyNotFoundException("Produto não encontrado.");

        product.Active = false;

        await context.SaveChangesAsync();
    }

    public async Task<object> GetAsync(Guid id)
    {
        var product = await context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            throw new KeyNotFoundException("Produto não encontrado.");

        return new
        {
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Active
        };
    }

    public async Task<IReadOnlyCollection<object>> GetAllAsync()
    {
        return await context.Products
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => (object)new
            {
                x.Id,
                x.Name,
                x.Description,
                x.Price,
                x.Active
            })
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<object>> GetMenuAsync(Guid unitId)
    {
        var products = await (
            from unitProduct in context.UnitProducts
            join product in context.Products
                on unitProduct.ProductId equals product.Id
            where unitProduct.UnitId == unitId
                && unitProduct.Available
                && product.Active
            orderby product.Name
            select new
            {
                product.Id,
                product.Name,
                product.Description,
                product.Price
            })
            .AsNoTracking()
            .ToListAsync();

        return products.Cast<object>().ToList();
    }

    public async Task SetAvailabilityAsync(
        Guid unitId,
        Guid productId,
        SetProductAvailabilityRequest request)
    {
        var unitExists = await context.Units
            .AnyAsync(x => x.Id == unitId);

        if (!unitExists)
            throw new KeyNotFoundException("Unidade não encontrada.");

        var productExists = await context.Products
            .AnyAsync(x => x.Id == productId);

        if (!productExists)
            throw new KeyNotFoundException("Produto não encontrado.");

        var unitProduct = await context.UnitProducts
            .FirstOrDefaultAsync(x =>
                x.UnitId == unitId &&
                x.ProductId == productId);

        if (unitProduct is null)
        {
            unitProduct = new UnitProduct
            {
                UnitId = unitId,
                ProductId = productId,
                Available = request.Available
            };

            context.UnitProducts.Add(unitProduct);
        }
        else
        {
            unitProduct.Available = request.Available;
        }

        await context.SaveChangesAsync();
    }
}