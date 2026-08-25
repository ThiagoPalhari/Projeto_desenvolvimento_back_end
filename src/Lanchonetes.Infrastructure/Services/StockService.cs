using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.Interfaces;
using Lanchonetes.Domain.Entities;
using Lanchonetes.Domain.Enums;
using Lanchonetes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lanchonetes.Infrastructure.Services;

public class StockService(AppDbContext context) : IStockService
{
    public async Task CreateMovementAsync(CreateStockMovementRequest request)
    {
        if (request.Quantity <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.");

        var unitExists = await context.Units
            .AnyAsync(x => x.Id == request.UnitId);

        if (!unitExists)
            throw new KeyNotFoundException("Unidade não encontrada.");

        var productExists = await context.Products
            .AnyAsync(x => x.Id == request.ProductId);

        if (!productExists)
            throw new KeyNotFoundException("Produto não encontrado.");

        var stock = await context.Stocks
            .FirstOrDefaultAsync(x =>
                x.UnitId == request.UnitId &&
                x.ProductId == request.ProductId);

        if (stock is null)
        {
            stock = new Stock
            {
                Id = Guid.NewGuid(),
                UnitId = request.UnitId,
                ProductId = request.ProductId,
                Quantity = 0
            };

            context.Stocks.Add(stock);
        }

        if (request.Type == StockMovementType.EXIT)
        {
            if (stock.Quantity < request.Quantity)
                throw new InvalidOperationException(
                    "Estoque insuficiente para realizar a saída.");

            stock.Quantity -= request.Quantity;
        }
        else if (request.Type == StockMovementType.ENTRY)
        {
            stock.Quantity += request.Quantity;
        }

        var movement = new StockMovement
        {
            Id = Guid.NewGuid(),
            UnitId = request.UnitId,
            ProductId = request.ProductId,
            Type = request.Type,
            Quantity = request.Quantity,
            Reference = request.Reference,
            CreatedAt = DateTime.UtcNow
        };

        context.StockMovements.Add(movement);

        await context.SaveChangesAsync();
    }

    public async Task<object> GetAsync(Guid unitId, Guid productId)
    {
        var stock = await context.Stocks
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.UnitId == unitId &&
                x.ProductId == productId);

        if (stock is null)
        {
            return new
            {
                UnitId = unitId,
                ProductId = productId,
                Quantity = 0m
            };
        }

        return new
        {
            stock.UnitId,
            stock.ProductId,
            stock.Quantity
        };
    }
}