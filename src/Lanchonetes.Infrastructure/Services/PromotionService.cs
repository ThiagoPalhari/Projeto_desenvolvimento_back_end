using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.Interfaces;
using Lanchonetes.Domain.Entities;
using Lanchonetes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lanchonetes.Infrastructure.Services;

public class PromotionService(AppDbContext context) : IPromotionService
{
    public async Task CreateAsync(CreatePromotionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Nome da promoção é obrigatório.");

        if (request.DiscountPercentage <= 0 ||
            request.DiscountPercentage > 100)
            throw new ArgumentException(
                "O desconto deve estar entre 0 e 100%.");

        if (request.EndsAt <= request.StartsAt)
            throw new ArgumentException(
                "A data de término deve ser posterior à data de início.");

        var promotion = new Promotion
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            DiscountPercentage = request.DiscountPercentage,
            StartsAt = request.StartsAt.Kind == DateTimeKind.Utc
                ? request.StartsAt
                : request.StartsAt.ToUniversalTime(),
            EndsAt = request.EndsAt.Kind == DateTimeKind.Utc
                ? request.EndsAt
                : request.EndsAt.ToUniversalTime(),
            Active = true
        };

        context.Promotions.Add(promotion);

        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(
        Guid id,
        UpdatePromotionRequest request)
    {
        var promotion = await context.Promotions
            .FirstOrDefaultAsync(x => x.Id == id);

        if (promotion is null)
            throw new KeyNotFoundException(
                "Promoção não encontrada.");

        if (request.DiscountPercentage <= 0 ||
            request.DiscountPercentage > 100)
            throw new ArgumentException(
                "O desconto deve estar entre 0 e 100%.");

        if (request.EndsAt <= request.StartsAt)
            throw new ArgumentException(
                "A data de término deve ser posterior à data de início.");

        promotion.Name = request.Name;
        promotion.DiscountPercentage = request.DiscountPercentage;

        promotion.StartsAt = request.StartsAt.Kind == DateTimeKind.Utc
            ? request.StartsAt
            : request.StartsAt.ToUniversalTime();

        promotion.EndsAt = request.EndsAt.Kind == DateTimeKind.Utc
            ? request.EndsAt
            : request.EndsAt.ToUniversalTime();

        promotion.Active = request.Active;

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var promotion = await context.Promotions
            .FirstOrDefaultAsync(x => x.Id == id);

        if (promotion is null)
            throw new KeyNotFoundException(
                "Promoção não encontrada.");

        promotion.Active = false;

        await context.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<object>> GetAllAsync()
    {
        return await context.Promotions
            .AsNoTracking()
            .OrderByDescending(x => x.StartsAt)
            .Select(x => (object)new
            {
                x.Id,
                x.Name,
                x.DiscountPercentage,
                x.StartsAt,
                x.EndsAt,
                x.Active
            })
            .ToListAsync();
    }

    public async Task<object> CalculateAsync(
        Guid customerId,
        Guid orderId)
    {
        var customerExists = await context.Users
            .AnyAsync(x => x.Id == customerId);

        if (!customerExists)
            throw new KeyNotFoundException(
                "Cliente não encontrado.");

        var order = await context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Id == orderId &&
                x.CustomerId == customerId);

        if (order is null)
            throw new KeyNotFoundException(
                "Pedido não encontrado.");

        var now = DateTime.UtcNow;

        var promotion = await context.Promotions
            .AsNoTracking()
            .Where(x =>
                x.Active &&
                x.StartsAt <= now &&
                x.EndsAt >= now)
            .OrderByDescending(x => x.DiscountPercentage)
            .FirstOrDefaultAsync();

        if (promotion is null)
        {
            return new
            {
                PromotionId = (Guid?)null,
                PromotionName = (string?)null,
                DiscountPercentage = 0m,
                OriginalTotal = order.Total,
                DiscountAmount = 0m,
                FinalTotal = order.Total
            };
        }

        var discountAmount =
            order.Total * promotion.DiscountPercentage / 100m;

        var finalTotal = order.Total - discountAmount;

        return new
        {
            PromotionId = (Guid?)promotion.Id,
            PromotionName = promotion.Name,
            promotion.DiscountPercentage,
            OriginalTotal = order.Total,
            DiscountAmount = discountAmount,
            FinalTotal = finalTotal
        };
    }
}