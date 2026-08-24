namespace Lanchonetes.Domain.Entities;
public class Consent { public Guid Id { get; set; } public Guid CustomerId { get; set; } public string Purpose { get; set; } = string.Empty; public bool Granted { get; set; } public DateTime GrantedAt { get; set; } public DateTime? RevokedAt { get; set; } }
