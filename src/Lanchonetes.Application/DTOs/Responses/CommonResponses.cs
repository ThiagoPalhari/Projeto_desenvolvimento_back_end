namespace Lanchonetes.Application.DTOs.Responses;
public record UserResponse(Guid Id, string Name, string Email, Guid RoleId, bool Active);
public record TokenResponse(string AccessToken, DateTime ExpiresAt);
public record OrderResponse(Guid Id, string Status, string Channel, decimal Total);
public record PaymentResponse(Guid Id, string Status, string ProviderTransactionId);
public record ReportResponse(string Name, decimal Value, int Count);
