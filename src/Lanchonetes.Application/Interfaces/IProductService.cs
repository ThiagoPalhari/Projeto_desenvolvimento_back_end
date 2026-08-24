using Lanchonetes.Application.DTOs.Requests;
namespace Lanchonetes.Application.Interfaces;
public interface IProductService { Task CreateAsync(CreateProductRequest request); Task UpdateAsync(Guid id, UpdateProductRequest request); Task DeleteAsync(Guid id); Task<object> GetAsync(Guid id); Task<IReadOnlyCollection<object>> GetAllAsync(); Task<IReadOnlyCollection<object>> GetMenuAsync(Guid unitId); Task SetAvailabilityAsync(Guid unitId, Guid productId, SetProductAvailabilityRequest request); }
