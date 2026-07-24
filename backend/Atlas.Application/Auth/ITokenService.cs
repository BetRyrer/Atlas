using Atlas.Domain.Entities;

namespace Atlas.Application.Auth;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}
