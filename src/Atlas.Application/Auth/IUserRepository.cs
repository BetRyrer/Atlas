using Atlas.Domain.Entities;

namespace Atlas.Application.Auth;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
}
