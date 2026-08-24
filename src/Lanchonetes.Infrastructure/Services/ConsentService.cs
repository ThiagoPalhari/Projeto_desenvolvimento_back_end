using Lanchonetes.Application.DTOs.Requests; using Lanchonetes.Application.Interfaces;
namespace Lanchonetes.Infrastructure.Services;
public class ConsentService : IConsentService { public Task CreateAsync(CreateConsentRequest request) => throw new NotImplementedException(); public Task RevokeAsync(Guid id, RevokeConsentRequest request) => throw new NotImplementedException(); public Task<IReadOnlyCollection<object>> GetByCustomerAsync(Guid customerId) => throw new NotImplementedException(); }
