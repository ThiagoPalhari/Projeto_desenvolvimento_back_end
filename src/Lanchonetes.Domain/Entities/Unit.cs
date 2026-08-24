namespace Lanchonetes.Domain.Entities;
public class Unit { public Guid Id { get; set; } public string Name { get; set; } = string.Empty; public string Cnpj { get; set; } = string.Empty; public string Address { get; set; } = string.Empty; public bool Active { get; set; } = true; }
