namespace Lanchonetes.Domain.Entities;
public class Promotion { public Guid Id { get; set; } public string Name { get; set; } = string.Empty; public decimal DiscountPercentage { get; set; } public DateTime StartsAt { get; set; } public DateTime EndsAt { get; set; } public bool Active { get; set; } = true; }
