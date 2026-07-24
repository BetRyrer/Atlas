using Atlas.Application.Auth.Dtos;

namespace Atlas.Application.Auth;

public interface IAuthService
{
    Task<AuthResultDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken);
}
