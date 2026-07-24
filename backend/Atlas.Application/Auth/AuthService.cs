using Atlas.Application.Auth.Dtos;
using Atlas.Application.Common.Exceptions;

namespace Atlas.Application.Auth;

public sealed class AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : IAuthService
{
    public async Task<AuthResultDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsernameAsync(dto.Username, cancellationToken);

        if (user is null || !passwordHasher.Verify(dto.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid username or password.");
        }

        var (token, expiresAt) = tokenService.GenerateToken(user);

        return new AuthResultDto
        {
            Token = token,
            Username = user.Username,
            DisplayName = user.DisplayName,
            ExpiresAt = expiresAt
        };
    }
}
