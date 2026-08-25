using Lanchonetes.Application.Interfaces;
using Lanchonetes.Domain.Entities;
using Lanchonetes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lanchonetes.Infrastructure.Services;

public class AuditService(AppDbContext context) : IAuditService
{
    public async Task RegisterAsync(
        Guid? userId,
        string action,
        string entity,
        string? entityId,
        string? details)
    {
        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Action = action,
            Entity = entity,
            EntityId = entityId,
            Details = details,
            CreatedAt = DateTime.UtcNow
        };

        context.AuditLogs.Add(auditLog);

        await context.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<object>> GetAsync(
    DateTime? from,
    DateTime? to)
    {
        var query = context.AuditLogs
            .AsNoTracking()
            .AsQueryable();

        if (from.HasValue)
        {
            var fromUtc = DateTime.SpecifyKind(
                from.Value,
                DateTimeKind.Utc);

            query = query.Where(x => x.CreatedAt >= fromUtc);
        }

        if (to.HasValue)
        {
            var endDateUtc = DateTime.SpecifyKind(
                to.Value.Date.AddDays(1),
                DateTimeKind.Utc);

            query = query.Where(x => x.CreatedAt < endDateUtc);
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new
            {
                x.Id,
                x.UserId,
                x.Action,
                x.Entity,
                x.EntityId,
                x.Details,
                x.CreatedAt
            })
            .Cast<object>()
            .ToListAsync();
    }
}