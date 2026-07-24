using Atlas.Application.Matrix;
using Atlas.Application.Matrix.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.Api.Controllers;

[ApiController]
[Route("api/matrix")]
public sealed class MatrixController(IMatrixService matrixService) : ControllerBase
{
    /// <summary>Returns the department-by-tool usage coverage matrix.</summary>
    [HttpGet]
    public async Task<ActionResult<MatrixDto>> Get(CancellationToken cancellationToken)
    {
        var matrix = await matrixService.GetMatrixAsync(cancellationToken);
        return Ok(matrix);
    }
}
