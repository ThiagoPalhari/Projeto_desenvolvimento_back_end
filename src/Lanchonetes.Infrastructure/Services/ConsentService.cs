using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.Interfaces;
using Lanchonetes.Domain.Entities;
using Lanchonetes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lanchonetes.Infrastructure.Services;

public class ConsentService(AppDbContext context) : IConsentService
{
    public async Task CreateAsync(CreateConsentRequest request)
    {
        var customerExists = await context.Users
            .AnyAsync(x => x.Id == request.CustomerId);

        if (!customerExists)
            throw new KeyNotFoundException("Cliente não encontrado.");

        var consent = new Consent
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            Purpose = request.Purpose,
            Granted = request.Granted,
            GrantedAt = request.Granted
                ? DateTime.UtcNow
                : DateTime.UtcNow
        };

        context.Consents.Add(consent);

        await context.SaveChangesAsync();
    }

    public async Task RevokeAsync(Guid id, RevokeConsentRequest request)
    {
        var consent = await context.Consents
            .FirstOrDefaultAsync(x => x.Id == id);

        if (consent is null)
            throw new KeyNotFoundException("Consentimento não encontrado.");

        if (!consent.Granted)
            throw new InvalidOperationException(
                "O consentimento já está revogado.");

        consent.Granted = false;
        consent.RevokedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<object>> GetByCustomerAsync(
        Guid customerId)
    {
        var customerExists = await context.Users
            .AnyAsync(x => x.Id == customerId);

        if (!customerExists)
            throw new KeyNotFoundException("Cliente não encontrado.");

        return await context.Consents
            .AsNoTracking()
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.GrantedAt)
            .Select(x => (object)new
            {
                x.Id,
                x.CustomerId,
                x.Purpose,
                x.Granted,
                x.GrantedAt,
                x.RevokedAt
            })
            .ToListAsync();
    }
}