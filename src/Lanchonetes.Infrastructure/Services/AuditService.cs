using Lanchonetes.Application.Interfaces;
namespace Lanchonetes.Infrastructure.Services;
public class AuditService : IAuditService { public Task RegisterAsync(Guid? userId, string action, string entity, string? entityId, string? details) => throw new NotImplementedException(); public Task<IReadOnlyCollection<object>> GetAsync(DateTime? from, DateTime? to) => throw new NotImplementedException(); }
