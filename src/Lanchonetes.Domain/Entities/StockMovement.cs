using Lanchonetes.Domain.Enums;
namespace Lanchonetes.Domain.Entities;
public class StockMovement { public Guid Id { get; set; } public Guid UnitId { get; set; } public Guid ProductId { get; set; } public StockMovementType Type { get; set; } public decimal Quantity { get; set; } public string? Reference { get; set; } public DateTime CreatedAt { get; set; } = DateTime.UtcNow; }
