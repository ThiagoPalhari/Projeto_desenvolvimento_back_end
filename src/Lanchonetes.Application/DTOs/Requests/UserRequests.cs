namespace Lanchonetes.Application.DTOs.Requests;
public record RegisterUserRequest(string Name, string Email, string Password, Guid RoleId);
public record LoginRequest(string Email, string Password);
public record UpdateUserRequest(string Name, string Email, Guid RoleId, bool Active);
