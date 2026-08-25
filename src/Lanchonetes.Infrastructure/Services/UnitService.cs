using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.Interfaces;
using Lanchonetes.Domain.Entities;
using Lanchonetes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lanchonetes.Infrastructure.Services;

public class UnitService(AppDbContext context) : IUnitService
{
    public async Task CreateAsync(CreateUnitRequest request)
    {
        var unit = new Unit
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Cnpj = request.Cnpj,
            Address = request.Address,
            Active = true
        };

        context.Units.Add(unit);

        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateUnitRequest request)
    {
        var unit = await context.Units
            .FirstOrDefaultAsync(x => x.Id == id);

        if (unit is null)
            throw new KeyNotFoundException("Unidade não encontrada.");

        unit.Name = request.Name;
        unit.Cnpj = request.Cnpj;
        unit.Address = request.Address;
        unit.Active = request.Active;

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var unit = await context.Units
            .FirstOrDefaultAsync(x => x.Id == id);

        if (unit is null)
            throw new KeyNotFoundException("Unidade não encontrada.");

        unit.Active = false;

        await context.SaveChangesAsync();
    }

    public async Task<object> GetAsync(Guid id)
    {
        var unit = await context.Units
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (unit is null)
            throw new KeyNotFoundException("Unidade não encontrada.");

        return new
        {
            unit.Id,
            unit.Name,
            unit.Cnpj,
            unit.Address,
            unit.Active
        };
    }

    public async Task<IReadOnlyCollection<object>> GetAllAsync()
    {
        return await context.Units
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => (object)new
            {
                x.Id,
                x.Name,
                x.Cnpj,
                x.Address,
                x.Active
            })
            .ToListAsync();
    }
}