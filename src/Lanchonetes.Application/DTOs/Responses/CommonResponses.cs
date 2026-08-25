namespace Lanchonetes.Application.DTOs.Responses;
public record TokenResponse(string AccessToken, DateTime ExpiresAt);
public record ReportResponse(string Name, decimal Value, int Count);
