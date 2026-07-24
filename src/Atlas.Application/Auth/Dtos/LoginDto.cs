namespace Atlas.Application.Auth.Dtos;

public sealed record LoginDto
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}
