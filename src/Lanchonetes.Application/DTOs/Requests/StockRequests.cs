using Lanchonetes.Domain.Enums;
namespace Lanchonetes.Application.DTOs.Requests;
public record CreateStockMovementRequest(Guid UnitId, Guid ProductId, StockMovementType Type, decimal Quantity, string? Reference);
