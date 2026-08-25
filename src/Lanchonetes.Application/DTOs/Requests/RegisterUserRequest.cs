namespace Lanchonetes.Application.DTOs.Requests;

public record RegisterUserRequest(
    string Name,
    string Email,
    string Password,
    Guid RoleId
);