using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.DTOs.Responses;
using System.Threading;

namespace Lanchonetes.Application.Interfaces;

public interface IUserService
{
    Task<UserResponse> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken);

    Task<LoginResponse> LoginAsync(LoginRequest request);
}