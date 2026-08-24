namespace Lanchonetes.Application.DTOs.Requests;
public record ProcessPaymentRequest(Guid OrderId, decimal Amount, string PaymentMethod);
public record PaymentCallbackRequest(Guid PaymentId, string ProviderTransactionId, string Status);
