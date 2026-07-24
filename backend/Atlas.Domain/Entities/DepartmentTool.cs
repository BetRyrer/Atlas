using Atlas.Domain.Enums;

namespace Atlas.Domain.Entities;

public sealed class DepartmentTool
{
    private DepartmentTool()
    {
    }

    public DepartmentTool(int departmentId, int toolId, UsageLevel usageLevel, string? referent, DateOnly? adoptedOn)
    {
        DepartmentId = departmentId;
        ToolId = toolId;
        UsageLevel = usageLevel;
        Referent = referent;
        AdoptedOn = adoptedOn;
    }

    public int DepartmentId { get; private set; }
    public int ToolId { get; private set; }
    public UsageLevel UsageLevel { get; private set; }
    public string? Referent { get; private set; }
    public DateOnly? AdoptedOn { get; private set; }

    public Department Department { get; private set; } = null!;
    public Tool Tool { get; private set; } = null!;
}
