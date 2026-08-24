using Lanchonetes.Application.Interfaces;
namespace Lanchonetes.Infrastructure.Services;
public class RoleService : IRoleService { public Task CreateAsync(string name) => throw new NotImplementedException(); public Task AssignPermissionAsync(Guid roleId, Guid permissionId) => throw new NotImplementedException(); public Task<IReadOnlyCollection<string>> GetPermissionsAsync(Guid roleId) => throw new NotImplementedException(); }
