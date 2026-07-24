using Atlas.Domain.Enums;

namespace Atlas.Application.Matrix.Dtos;

public sealed record MatrixCellDto
{
    public required int ToolId { get; init; }
    public UsageLevel? UsageLevel { get; init; }
}
