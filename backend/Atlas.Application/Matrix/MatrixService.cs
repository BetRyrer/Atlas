using Atlas.Application.Departments;
using Atlas.Application.Matrix.Dtos;
using Atlas.Application.Tools;

namespace Atlas.Application.Matrix;

public sealed class MatrixService(
    IDepartmentRepository departmentRepository,
    IToolRepository toolRepository) : IMatrixService
{
    public async Task<MatrixDto> GetMatrixAsync(CancellationToken cancellationToken)
    {
        var departments = await departmentRepository.GetAllAsync(cancellationToken);
        var tools = await toolRepository.GetAllAsync(cancellationToken);
        var links = await departmentRepository.GetAllLinksAsync(cancellationToken);

        var linksByDepartment = links
            .GroupBy(link => link.DepartmentId)
            .ToDictionary(group => group.Key, group => group.ToDictionary(link => link.ToolId, link => link.UsageLevel));

        var rows = departments.Select(department =>
        {
            linksByDepartment.TryGetValue(department.Id, out var departmentLinks);

            var cells = tools.Select(tool => new MatrixCellDto
            {
                ToolId = tool.Id,
                UsageLevel = departmentLinks is not null && departmentLinks.TryGetValue(tool.Id, out var usageLevel)
                    ? usageLevel
                    : null
            }).ToList();

            return new MatrixRowDto
            {
                DepartmentId = department.Id,
                DepartmentName = department.Name,
                Cells = cells
            };
        }).ToList();

        return new MatrixDto
        {
            Tools = tools.Select(tool => new MatrixToolDto { ToolId = tool.Id, ToolName = tool.Name }).ToList(),
            Rows = rows
        };
    }
}
