using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lanchonetes.Application.Interfaces;
using Lanchonetes.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Lanchonetes.Infrastructure.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateToken(User user)
    {
        var jwt = configuration.GetSection("Jwt");

        var key = jwt["Key"]
            ?? throw new InvalidOperationException("JWT Key não configurada.");

        var issuer = jwt["Issuer"]
            ?? throw new InvalidOperationException("JWT Issuer não configurado.");

        var audience = jwt["Audience"]
            ?? throw new InvalidOperationException("JWT Audience não configurada.");

        var expirationMinutes = int.TryParse(jwt["ExpirationMinutes"], out var minutes)
            ? minutes
            : 60;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role?.Name ?? string.Empty)
        };

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}