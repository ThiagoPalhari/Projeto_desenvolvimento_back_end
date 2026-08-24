namespace Lanchonetes.Domain.Entities;
public class LoyaltyAccount { public Guid Id { get; set; } public Guid CustomerId { get; set; } public User? Customer { get; set; } public int Points { get; set; } }
