using Lanchonetes.Domain.Enums;
namespace Lanchonetes.Application.DTOs.Requests;
public record CreateOrderItemRequest(Guid ProductId, int Quantity);
public record CreateOrderRequest(Guid CustomerId, Guid UnitId, OrderChannel Channel, IReadOnlyCollection<CreateOrderItemRequest> Items);
public record UpdateOrderStatusRequest(OrderStatus Status);
public record CancelOrderRequest(string Reason);
