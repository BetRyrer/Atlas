using Atlas.Application.Departments.Dtos;

namespace Atlas.Application.Departments;

public interface IDepartmentService
{
    Task<IReadOnlyCollection<DepartmentListDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<DepartmentDetailDto> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<DepartmentToolLinkDto>> GetToolsAsync(int departmentId, CancellationToken cancellationToken);

    Task<DepartmentToolLinkDto> LinkToolAsync(int departmentId, LinkToolDto dto, CancellationToken cancellationToken);

    Task UnlinkToolAsync(int departmentId, int toolId, CancellationToken cancellationToken);
}
