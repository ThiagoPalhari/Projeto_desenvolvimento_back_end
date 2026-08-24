using Lanchonetes.Domain.Enums;
namespace Lanchonetes.Domain.Entities;
public class Payment { public Guid Id { get; set; } public Guid OrderId { get; set; } public Order? Order { get; set; } public string ProviderTransactionId { get; set; } = string.Empty; public decimal Amount { get; set; } public PaymentStatus Status { get; set; } public DateTime CreatedAt { get; set; } = DateTime.UtcNow; }
