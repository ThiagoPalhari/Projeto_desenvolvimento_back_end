using Lanchonetes.Application.DTOs.Requests;
namespace Lanchonetes.Application.Interfaces;
public interface IPromotionService { Task CreateAsync(CreatePromotionRequest request); Task UpdateAsync(Guid id, UpdatePromotionRequest request); Task DeleteAsync(Guid id); Task<IReadOnlyCollection<object>> GetAllAsync(); Task<object> CalculateAsync(Guid customerId, Guid orderId); }
