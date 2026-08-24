namespace Lanchonetes.Application.Interfaces;
public interface IRoleService { Task CreateAsync(string name); Task AssignPermissionAsync(Guid roleId, Guid permissionId); Task<IReadOnlyCollection<string>> GetPermissionsAsync(Guid roleId); }
