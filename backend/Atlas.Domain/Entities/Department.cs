namespace Atlas.Domain.Entities;

public sealed class Department
{
    private readonly List<DepartmentTool> _departmentTools = [];

    private Department()
    {
    }

    public Department(string name, string description, int headCount)
    {
        Name = name;
        Description = description;
        HeadCount = headCount;
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int HeadCount { get; private set; }
    public IReadOnlyCollection<DepartmentTool> DepartmentTools => _departmentTools.AsReadOnly();

    public void UpdateDetails(string name, string description, int headCount)
    {
        Name = name;
        Description = description;
        HeadCount = headCount;
    }
}
