namespace Lanchonetes.Domain.Entities;

public class LoyaltyTransaction
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public int Points { get; set; }

    public string Type { get; set; } = string.Empty;

    public string? Reference { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}