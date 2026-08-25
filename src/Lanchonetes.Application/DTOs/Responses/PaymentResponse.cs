namespace Lanchonetes.Application.DTOs.Responses;

public record PaymentResponse(
    Guid Id,
    string Status,
    string ProviderTransactionId);