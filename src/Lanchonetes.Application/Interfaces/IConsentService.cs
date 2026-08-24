using Lanchonetes.Application.DTOs.Requests;
namespace Lanchonetes.Application.Interfaces;
public interface IConsentService { Task CreateAsync(CreateConsentRequest request); Task RevokeAsync(Guid id, RevokeConsentRequest request); Task<IReadOnlyCollection<object>> GetByCustomerAsync(Guid customerId); }
