using Lanchonetes.Application.Interfaces;
using Lanchonetes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lanchonetes.Infrastructure.Services;

public class ReportService(AppDbContext context) : IReportService
{
    public async Task<IReadOnlyCollection<object>> SalesAsync(
        DateTime? from,
        DateTime? to,
        Guid? unitId)
    {
        var query = context.Orders
            .AsNoTracking()
            .AsQueryable();

        if (from.HasValue)
        {
            var startDate = DateTime.SpecifyKind(
                from.Value,
                DateTimeKind.Utc);

            query = query.Where(x => x.CreatedAt >= startDate);
        }

        if (to.HasValue)
        {
            var endDate = DateTime.SpecifyKind(
                to.Value.Date.AddDays(1),
                DateTimeKind.Utc);

            query = query.Where(x => x.CreatedAt < endDate);
        }

        if (unitId.HasValue)
        {
            query = query.Where(x => x.UnitId == unitId.Value);
        }

        var result = await query
            .GroupBy(x => x.UnitId)
            .Select(g => new
            {
                UnitId = g.Key,
                Orders = g.Count(),
                Subtotal = g.Sum(x => x.Subtotal),
                Discount = g.Sum(x => x.Discount),
                Total = g.Sum(x => x.Total)
            })
            .OrderBy(x => x.UnitId)
            .ToListAsync();

        return result.Cast<object>().ToList();
    }

    public async Task<IReadOnlyCollection<object>> OrdersAsync(
        DateTime? from,
        DateTime? to,
        Guid? unitId)
    {
        var query = context.Orders
            .AsNoTracking()
            .AsQueryable();

        if (from.HasValue)
        {
            var startDate = DateTime.SpecifyKind(
                from.Value,
                DateTimeKind.Utc);

            query = query.Where(x => x.CreatedAt >= startDate);
        }

        if (to.HasValue)
        {
            var endDate = DateTime.SpecifyKind(
                to.Value.Date.AddDays(1),
                DateTimeKind.Utc);

            query = query.Where(x => x.CreatedAt < endDate);
        }

        if (unitId.HasValue)
        {
            query = query.Where(x => x.UnitId == unitId.Value);
        }

        var result = await query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new
            {
                x.Id,
                x.CustomerId,
                x.UnitId,
                Status = x.Status.ToString(),
                Channel = x.Channel.ToString(),
                x.Subtotal,
                x.Discount,
                x.Total,
                x.CreatedAt
            })
            .ToListAsync();

        return result.Cast<object>().ToList();
    }

    public async Task<IReadOnlyCollection<object>> ProductsAsync(
        DateTime? from,
        DateTime? to,
        Guid? unitId)
    {
        var query = context.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .AsQueryable();

        if (from.HasValue)
        {
            var startDate = DateTime.SpecifyKind(
                from.Value,
                DateTimeKind.Utc);

            query = query.Where(x => x.CreatedAt >= startDate);
        }

        if (to.HasValue)
        {
            var endDate = DateTime.SpecifyKind(
                to.Value.Date.AddDays(1),
                DateTimeKind.Utc);

            query = query.Where(x => x.CreatedAt < endDate);
        }

        if (unitId.HasValue)
        {
            query = query.Where(x => x.UnitId == unitId.Value);
        }

        var result = await query
            .SelectMany(x => x.Items)
            .GroupBy(x => new
            {
                x.ProductId,
                ProductName = x.Product!.Name
            })
            .Select(g => new
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.ProductName,
                Quantity = g.Sum(x => x.Quantity),
                Revenue = g.Sum(x => x.Total)
            })
            .OrderByDescending(x => x.Quantity)
            .ToListAsync();

        return result.Cast<object>().ToList();
    }
}