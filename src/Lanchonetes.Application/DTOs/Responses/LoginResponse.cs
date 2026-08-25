namespace Lanchonetes.Application.DTOs.Responses;

public record LoginResponse(
    string AccessToken,
    Guid UserId,
    string Name,
    string Email,
    string Role
);