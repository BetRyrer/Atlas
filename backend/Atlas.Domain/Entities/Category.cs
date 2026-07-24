namespace Atlas.Domain.Entities;

public sealed class Category
{
    private readonly List<Tool> _tools = [];

    private Category()
    {
    }

    public Category(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public IReadOnlyCollection<Tool> Tools => _tools.AsReadOnly();
}
