using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.DTOs.Responses;
using Lanchonetes.Domain.Enums;
namespace Lanchonetes.Application.Interfaces;
public interface IOrderService { Task<OrderResponse> CreateAsync(CreateOrderRequest request); Task<OrderResponse> GetAsync(Guid id); Task<IReadOnlyCollection<OrderResponse>> GetAllAsync(OrderChannel? channel); Task<OrderResponse> UpdateStatusAsync(Guid id, UpdateOrderStatusRequest request); Task CancelAsync(Guid id, CancelOrderRequest request); }
