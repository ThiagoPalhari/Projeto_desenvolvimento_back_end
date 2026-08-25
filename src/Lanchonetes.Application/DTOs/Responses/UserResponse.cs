namespace Lanchonetes.Application.DTOs.Responses;

public record UserResponse(
    Guid Id,
    string Name,
    string Email,
    Guid RoleId,
    string Role,
    bool Active,
    DateTime CreatedAt
);