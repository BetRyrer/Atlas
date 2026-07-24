using Atlas.Application.Matrix.Dtos;

namespace Atlas.Application.Matrix;

public interface IMatrixService
{
    Task<MatrixDto> GetMatrixAsync(CancellationToken cancellationToken);
}
