using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.DTOs.Responses;
namespace Lanchonetes.Application.Interfaces;
public interface IUserService { Task<UserResponse> RegisterAsync(RegisterUserRequest request); Task<TokenResponse> LoginAsync(LoginRequest request); Task<UserResponse> GetAsync(Guid id); Task<IReadOnlyCollection<UserResponse>> GetAllAsync(); Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest request); Task DeleteAsync(Guid id); }
