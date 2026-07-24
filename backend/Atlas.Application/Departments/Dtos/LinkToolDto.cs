using Atlas.Domain.Enums;

namespace Atlas.Application.Departments.Dtos;

public sealed record LinkToolDto
{
    public required int ToolId { get; init; }
    public required UsageLevel UsageLevel { get; init; }
    public string? Referent { get; init; }
    public DateOnly? AdoptedOn { get; init; }
}
