using Atlas.Domain.Enums;

namespace Atlas.Application.Departments.Dtos;

public sealed record DepartmentToolLinkDto
{
    public required int ToolId { get; init; }
    public required string ToolName { get; init; }
    public required string CategoryName { get; init; }
    public string? LogoUrl { get; init; }
    public required UsageLevel UsageLevel { get; init; }
    public string? Referent { get; init; }
    public DateOnly? AdoptedOn { get; init; }
}
