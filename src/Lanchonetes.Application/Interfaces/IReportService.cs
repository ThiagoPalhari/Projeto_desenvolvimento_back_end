namespace Lanchonetes.Application.Interfaces;
public interface IReportService { Task<IReadOnlyCollection<object>> SalesAsync(DateTime? from, DateTime? to, Guid? unitId); Task<IReadOnlyCollection<object>> OrdersAsync(DateTime? from, DateTime? to, Guid? unitId); Task<IReadOnlyCollection<object>> ProductsAsync(DateTime? from, DateTime? to, Guid? unitId); }
