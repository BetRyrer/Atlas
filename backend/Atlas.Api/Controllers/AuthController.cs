using Atlas.Application.Auth;
using Atlas.Application.Auth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.Api.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>Authenticates a user and returns a JWT bearer token.</summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResultDto>> Login(LoginDto dto, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(dto, cancellationToken);
        return Ok(result);
    }
}
