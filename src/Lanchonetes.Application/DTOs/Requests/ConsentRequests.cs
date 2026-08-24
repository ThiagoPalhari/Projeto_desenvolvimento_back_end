namespace Lanchonetes.Application.DTOs.Requests;
public record CreateConsentRequest(Guid CustomerId, string Purpose, bool Granted);
public record RevokeConsentRequest(string Reason);
