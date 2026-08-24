using Lanchonetes.Application.DTOs.Requests; using Lanchonetes.Application.Interfaces;
namespace Lanchonetes.Infrastructure.Services;
public class StockService : IStockService { public Task CreateMovementAsync(CreateStockMovementRequest request) => throw new NotImplementedException(); public Task<object> GetAsync(Guid unitId, Guid productId) => throw new NotImplementedException(); }
