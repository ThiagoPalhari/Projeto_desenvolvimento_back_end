namespace Lanchonetes.Domain.Entities;
public class Stock { public Guid Id { get; set; } public Guid UnitId { get; set; } public Unit? Unit { get; set; } public Guid ProductId { get; set; } public Product? Product { get; set; } public decimal Quantity { get; set; } }
