namespace Lanchonetes.Application.DTOs.Requests;

public record UpdateUserRequest(string Name, string Email, Guid RoleId, bool Active);
