namespace Lanchonetes.Application.DTOs.Requests;
public record CreatePromotionRequest(string Name, decimal DiscountPercentage, DateTime StartsAt, DateTime EndsAt);
public record UpdatePromotionRequest(string Name, decimal DiscountPercentage, DateTime StartsAt, DateTime EndsAt, bool Active);
