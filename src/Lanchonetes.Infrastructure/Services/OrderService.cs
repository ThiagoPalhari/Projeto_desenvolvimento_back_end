using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.DTOs.Responses;
using Lanchonetes.Application.Interfaces;
using Lanchonetes.Domain.Entities;
using Lanchonetes.Domain.Enums;
using Lanchonetes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lanchonetes.Infrastructure.Services;

public class OrderService(
    AppDbContext context,
    IAuditService auditService) : IOrderService
{
    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
    {
        var customerExists = await context.Users
            .AnyAsync(x => x.Id == request.CustomerId && x.Active);

        if (!customerExists)
            throw new InvalidOperationException("Cliente não encontrado ou inativo.");

        var unitExists = await context.Units
            .AnyAsync(x => x.Id == request.UnitId && x.Active);

        if (!unitExists)
            throw new InvalidOperationException("Unidade não encontrada ou inativa.");

        if (request.Items == null || request.Items.Count == 0)
            throw new InvalidOperationException("O pedido deve possuir pelo menos um item.");

        var productIds = request.Items
            .Select(x => x.ProductId)
            .Distinct()
            .ToList();

        var products = await context.Products
            .Where(x => productIds.Contains(x.Id) && x.Active)
            .ToDictionaryAsync(x => x.Id);

        foreach (var item in request.Items)
        {
            if (!products.ContainsKey(item.ProductId))
                throw new InvalidOperationException(
                    $"Produto não encontrado ou inativo: {item.ProductId}");

            if (item.Quantity <= 0)
                throw new InvalidOperationException(
                    "A quantidade do produto deve ser maior que zero.");

            var stock = await context.Stocks
                .FirstOrDefaultAsync(x => x.UnitId == request.UnitId && x.ProductId == item.ProductId);

            if (stock is null || stock.Quantity < item.Quantity)
                throw new InvalidOperationException(
                    $"Estoque insuficiente para o produto {item.ProductId} na unidade {request.UnitId}.");
        }

        var promotion = await GetApplicablePromotionAsync();

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            UnitId = request.UnitId,
            Channel = request.Channel,
            Status = OrderStatus.CREATED,
            Discount = 0m,
            CreatedAt = DateTime.UtcNow
        };

        decimal subtotal = 0;

        foreach (var requestItem in request.Items)
        {
            var product = products[requestItem.ProductId];
            var itemTotal = product.Price * requestItem.Quantity;

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = requestItem.Quantity,
                UnitPrice = product.Price,
                Total = itemTotal
            };

            order.Items.Add(orderItem);
            subtotal += itemTotal;
        }

        order.Subtotal = subtotal;
        order.Discount = promotion is null ? 0m : promotion.DiscountPercentage;
        order.Total = subtotal - (subtotal * order.Discount / 100m);

        context.Orders.Add(order);

        foreach (var item in request.Items)
        {
            var stock = await context.Stocks
                .FirstOrDefaultAsync(x => x.UnitId == request.UnitId && x.ProductId == item.ProductId);

            if (stock is not null)
            {
                stock.Quantity -= item.Quantity;
            }
        }

        await context.SaveChangesAsync();

        await auditService.RegisterAsync(
            null,
            "CREATE_ORDER",
            nameof(Order),
            order.Id.ToString(),
            $"Pedido criado para cliente {request.CustomerId} na unidade {request.UnitId}.");

        return new OrderResponse(
            order.Id,
            order.Status.ToString(),
            order.Channel.ToString(),
            order.Total);
    }

    public async Task<OrderResponse> GetAsync(Guid id)
    {
        var order = await context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (order is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        return new OrderResponse(
            order.Id,
            order.Status.ToString(),
            order.Channel.ToString(),
            order.Total);
    }

    public async Task<IReadOnlyCollection<OrderResponse>> GetAllAsync(
        OrderChannel? channel)
    {
        var query = context.Orders
            .AsNoTracking()
            .AsQueryable();

        if (channel.HasValue)
            query = query.Where(x => x.Channel == channel.Value);

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new OrderResponse(
                x.Id,
                x.Status.ToString(),
                x.Channel.ToString(),
                x.Total))
            .ToListAsync();
    }

    public async Task<OrderResponse> UpdateStatusAsync(
        Guid id,
        UpdateOrderStatusRequest request)
    {
        var order = await context.Orders
            .FirstOrDefaultAsync(x => x.Id == id);

        if (order is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        order.Status = request.Status;
        order.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        await auditService.RegisterAsync(
            null,
            "UPDATE_ORDER_STATUS",
            nameof(Order),
            order.Id.ToString(),
            $"Status do pedido atualizado para {request.Status}.");

        return new OrderResponse(
            order.Id,
            order.Status.ToString(),
            order.Channel.ToString(),
            order.Total);
    }

    public async Task CancelAsync(
        Guid id,
        CancelOrderRequest request)
    {
        var order = await context.Orders
            .FirstOrDefaultAsync(x => x.Id == id);

        if (order is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        order.Status = OrderStatus.CANCELLED;
        order.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        await auditService.RegisterAsync(
            null,
            "CANCEL_ORDER",
            nameof(Order),
            order.Id.ToString(),
            $"Pedido cancelado. Motivo: {request.Reason}.");
    }

    private async Task<Promotion?> GetApplicablePromotionAsync()
    {
        var now = DateTime.UtcNow;

        return await context.Promotions
            .AsNoTracking()
            .Where(x =>
                x.Active &&
                x.StartsAt <= now &&
                x.EndsAt >= now)
            .OrderByDescending(x => x.DiscountPercentage)
            .FirstOrDefaultAsync();
    }
}