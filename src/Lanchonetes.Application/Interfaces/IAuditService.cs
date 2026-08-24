namespace Lanchonetes.Application.Interfaces;
public interface IAuditService { Task RegisterAsync(Guid? userId, string action, string entity, string? entityId, string? details); Task<IReadOnlyCollection<object>> GetAsync(DateTime? from, DateTime? to); }
