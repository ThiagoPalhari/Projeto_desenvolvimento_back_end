using Lanchonetes.Application.DTOs.Requests;
namespace Lanchonetes.Application.Interfaces;
public interface IStockService { Task CreateMovementAsync(CreateStockMovementRequest request); Task<object> GetAsync(Guid unitId, Guid productId); }
