namespace Atlas.Domain.Entities;

public sealed class User
{
    private User()
    {
    }

    public User(string username, string displayName, string passwordHash)
    {
        Username = username;
        DisplayName = displayName;
        PasswordHash = passwordHash;
    }

    public int Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
}
