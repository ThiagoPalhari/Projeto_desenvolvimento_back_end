using Lanchonetes.Application.Interfaces;
using Lanchonetes.Domain.Entities;
using Lanchonetes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lanchonetes.Infrastructure.Services;

public class RoleService(AppDbContext context) : IRoleService
{
    public async Task CreateAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome da role é obrigatório.");

        var exists = await context.Roles
            .AnyAsync(x => x.Name == name);

        if (exists)
            throw new InvalidOperationException("Role já existe.");

        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = name.Trim()
        };

        context.Roles.Add(role);

        await context.SaveChangesAsync();
    }

    public async Task AssignPermissionAsync(Guid roleId, Guid permissionId)
    {
        var roleExists = await context.Roles
            .AnyAsync(x => x.Id == roleId);

        if (!roleExists)
            throw new KeyNotFoundException("Role não encontrada.");

        var permissionExists = await context.Permissions
            .AnyAsync(x => x.Id == permissionId);

        if (!permissionExists)
            throw new KeyNotFoundException("Permissão não encontrada.");

        var alreadyAssigned = await context.RolePermissions
            .AnyAsync(x =>
                x.RoleId == roleId &&
                x.PermissionId == permissionId);

        if (alreadyAssigned)
            return;

        var rolePermission = new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId
        };

        context.RolePermissions.Add(rolePermission);

        await context.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<string>> GetPermissionsAsync(Guid roleId)
    {
        var roleExists = await context.Roles
            .AnyAsync(x => x.Id == roleId);

        if (!roleExists)
            throw new KeyNotFoundException("Role não encontrada.");

        return await context.RolePermissions
            .AsNoTracking()
            .Where(x => x.RoleId == roleId)
            .Select(x => x.Permission!.Name)
            .OrderBy(x => x)
            .ToListAsync();
    }
}