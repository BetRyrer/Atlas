namespace Atlas.Application.Matrix.Dtos;

public sealed record MatrixToolDto
{
    public required int ToolId { get; init; }
    public required string ToolName { get; init; }
}
