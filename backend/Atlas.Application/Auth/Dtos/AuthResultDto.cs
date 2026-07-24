namespace Atlas.Application.Auth.Dtos;

public sealed record AuthResultDto
{
    public required string Token { get; init; }
    public required string Username { get; init; }
    public required string DisplayName { get; init; }
    public required DateTime ExpiresAt { get; init; }
}
