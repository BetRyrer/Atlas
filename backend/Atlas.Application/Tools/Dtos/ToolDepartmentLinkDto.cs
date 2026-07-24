using Atlas.Domain.Enums;

namespace Atlas.Application.Tools.Dtos;

public sealed record ToolDepartmentLinkDto
{
    public required int DepartmentId { get; init; }
    public required string DepartmentName { get; init; }
    public required UsageLevel UsageLevel { get; init; }
    public string? Referent { get; init; }
    public DateOnly? AdoptedOn { get; init; }
}
