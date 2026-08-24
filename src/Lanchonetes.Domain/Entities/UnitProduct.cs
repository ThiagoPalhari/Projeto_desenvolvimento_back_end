namespace Lanchonetes.Domain.Entities;
public class UnitProduct { public Guid UnitId { get; set; } public Unit? Unit { get; set; } public Guid ProductId { get; set; } public Product? Product { get; set; } public bool Available { get; set; } = true; }
