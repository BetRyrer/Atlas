namespace Atlas.Application.Matrix.Dtos;

public sealed record MatrixDto
{
    public required IReadOnlyCollection<MatrixToolDto> Tools { get; init; }
    public required IReadOnlyCollection<MatrixRowDto> Rows { get; init; }
}
