using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.Interfaces;
using Lanchonetes.Domain.Entities;
using Lanchonetes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lanchonetes.Infrastructure.Services;

public class LoyaltyService(AppDbContext context) : ILoyaltyService
{
    public async Task<object> GetAccountAsync(Guid customerId)
    {
        var customerExists = await context.Users
            .AnyAsync(x => x.Id == customerId);

        if (!customerExists)
            throw new KeyNotFoundException("Cliente não encontrado.");

        var account = await context.LoyaltyAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);

        if (account is null)
        {
            account = new LoyaltyAccount
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Points = 0
            };

            context.LoyaltyAccounts.Add(account);
            await context.SaveChangesAsync();
        }

        return new
        {
            account.Id,
            account.CustomerId,
            account.Points
        };
    }

    public async Task AddPointsAsync(
        Guid customerId,
        int points,
        string? reference)
    {
        if (points <= 0)
            throw new ArgumentException("A quantidade de pontos deve ser maior que zero.");

        var customerExists = await context.Users
            .AnyAsync(x => x.Id == customerId);

        if (!customerExists)
            throw new KeyNotFoundException("Cliente não encontrado.");

        var account = await context.LoyaltyAccounts
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);

        if (account is null)
        {
            account = new LoyaltyAccount
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Points = 0
            };

            context.LoyaltyAccounts.Add(account);
        }

        account.Points += points;

        var transaction = new LoyaltyTransaction
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Points = points,
            Type = "EARN",
            Reference = reference,
            CreatedAt = DateTime.UtcNow
        };

        context.LoyaltyTransactions.Add(transaction);

        await context.SaveChangesAsync();
    }

    public async Task RedeemAsync(
        Guid customerId,
        RedeemPointsRequest request)
    {
        if (request.Points <= 0)
            throw new ArgumentException("A quantidade de pontos deve ser maior que zero.");

        var account = await context.LoyaltyAccounts
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);

        if (account is null)
            throw new KeyNotFoundException("Conta de fidelidade não encontrada.");

        if (account.Points < request.Points)
            throw new InvalidOperationException("Pontos insuficientes.");

        account.Points -= request.Points;

        var transaction = new LoyaltyTransaction
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Points = -request.Points,
            Type = "REDEEM",
            Reference = request.Reference,
            CreatedAt = DateTime.UtcNow
        };

        context.LoyaltyTransactions.Add(transaction);

        await context.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<object>> GetTransactionsAsync(
        Guid customerId)
    {
        var customerExists = await context.Users
            .AnyAsync(x => x.Id == customerId);

        if (!customerExists)
            throw new KeyNotFoundException("Cliente não encontrado.");

        return await context.LoyaltyTransactions
            .AsNoTracking()
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => (object)new
            {
                x.Id,
                x.CustomerId,
                x.Points,
                x.Type,
                x.Reference,
                x.CreatedAt
            })
            .ToListAsync();
    }
}