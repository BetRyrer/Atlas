using Atlas.Application.Auth;
using Atlas.Application.Auth.Dtos;
using Atlas.Application.Common.Exceptions;
using Atlas.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Atlas.Application.Tests.Auth;

public sealed class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly Mock<ITokenService> _tokenService = new();
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _sut = new AuthService(_userRepository.Object, _passwordHasher.Object, _tokenService.Object);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsAuthResult()
    {
        var user = new User("admin", "Camille Dubois", "hashed-password");
        var expiresAt = DateTime.UtcNow.AddHours(1);

        _userRepository.Setup(repo => repo.GetByUsernameAsync("admin", It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _passwordHasher.Setup(hasher => hasher.Verify("secret", "hashed-password")).Returns(true);
        _tokenService.Setup(service => service.GenerateToken(user)).Returns(("jwt-token", expiresAt));

        var dto = new LoginDto { Username = "admin", Password = "secret" };

        var result = await _sut.LoginAsync(dto, CancellationToken.None);

        result.Token.Should().Be("jwt-token");
        result.Username.Should().Be("admin");
        result.DisplayName.Should().Be("Camille Dubois");
        result.ExpiresAt.Should().Be(expiresAt);
    }

    [Fact]
    public async Task LoginAsync_UnknownUsername_ThrowsUnauthorizedException()
    {
        _userRepository.Setup(repo => repo.GetByUsernameAsync("unknown", It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

        var dto = new LoginDto { Username = "unknown", Password = "secret" };

        var act = () => _sut.LoginAsync(dto, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedException>();
        _tokenService.Verify(service => service.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WrongPassword_ThrowsUnauthorizedException()
    {
        var user = new User("admin", "Camille Dubois", "hashed-password");

        _userRepository.Setup(repo => repo.GetByUsernameAsync("admin", It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _passwordHasher.Setup(hasher => hasher.Verify("wrong", "hashed-password")).Returns(false);

        var dto = new LoginDto { Username = "admin", Password = "wrong" };

        var act = () => _sut.LoginAsync(dto, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedException>();
        _tokenService.Verify(service => service.GenerateToken(It.IsAny<User>()), Times.Never);
    }
}
