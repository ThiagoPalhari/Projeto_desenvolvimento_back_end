using Lanchonetes.Application.DTOs.Requests;
namespace Lanchonetes.Application.Interfaces;
public interface ILoyaltyService { Task<object> GetAccountAsync(Guid customerId); Task AddPointsAsync(Guid customerId, int points, string? reference); Task RedeemAsync(Guid customerId, RedeemPointsRequest request); Task<IReadOnlyCollection<object>> GetTransactionsAsync(Guid customerId); }
