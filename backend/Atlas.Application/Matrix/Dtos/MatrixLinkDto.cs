using Atlas.Domain.Enums;

namespace Atlas.Application.Matrix.Dtos;

public sealed record MatrixLinkDto
{
    public required int DepartmentId { get; init; }
    public required int ToolId { get; init; }
    public required UsageLevel UsageLevel { get; init; }
}
