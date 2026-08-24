using Lanchonetes.Application.Interfaces;
namespace Lanchonetes.Infrastructure.Services;
public class ReportService : IReportService { public Task<IReadOnlyCollection<object>> SalesAsync(DateTime? from, DateTime? to, Guid? unitId) => throw new NotImplementedException(); public Task<IReadOnlyCollection<object>> OrdersAsync(DateTime? from, DateTime? to, Guid? unitId) => throw new NotImplementedException(); public Task<IReadOnlyCollection<object>> ProductsAsync(DateTime? from, DateTime? to, Guid? unitId) => throw new NotImplementedException(); }
