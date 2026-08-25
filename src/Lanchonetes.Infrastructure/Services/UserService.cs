using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.DTOs.Responses;
using Lanchonetes.Application.Interfaces;
using System.Threading;
using Lanchonetes.Domain.Entities;
using Lanchonetes.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lanchonetes.Application.Services;

public class UserService(
    AppDbContext context,
    IPasswordHasher<User> passwordHasher,
    ITokenService tokenService) : IUserService
{
    public async Task<UserResponse> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var emailExists = await context.Users
            .AnyAsync(x => x.Email == email, cancellationToken);

        if (emailExists)
            throw new InvalidOperationException("E-mail já cadastrado.");

        var role = await context.Roles
            .FirstOrDefaultAsync(x => x.Id == request.RoleId, cancellationToken);

        if (role is null)
            throw new InvalidOperationException("Perfil informado não existe.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Email = email,
            RoleId = role.Id,
            Active = true,
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

        context.Users.Add(user);

        await context.SaveChangesAsync(cancellationToken);

        return new UserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.RoleId,
            role.Name,
            user.Active,
            user.CreatedAt);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == email);

        if (user is null || !user.Active)
            throw new UnauthorizedAccessException("Credenciais inválidas.");

        var passwordResult = passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (passwordResult == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Credenciais inválidas.");

        var accessToken = tokenService.GenerateToken(user);

        return new LoginResponse(
            accessToken,
            user.Id,
            user.Name,
            user.Email,
            user.Role?.Name ?? string.Empty);
    }
}