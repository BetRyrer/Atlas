namespace Atlas.Application.Matrix.Dtos;

public sealed record MatrixRowDto
{
    public required int DepartmentId { get; init; }
    public required string DepartmentName { get; init; }
    public required IReadOnlyCollection<MatrixCellDto> Cells { get; init; }
}
