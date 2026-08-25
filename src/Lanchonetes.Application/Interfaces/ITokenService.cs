using Lanchonetes.Domain.Entities;

namespace Lanchonetes.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}