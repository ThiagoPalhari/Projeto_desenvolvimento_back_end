namespace Lanchonetes.Application.DTOs.Responses;

public record OrderResponse(
    Guid Id,
    string Status,
    string Channel,
    decimal Total);