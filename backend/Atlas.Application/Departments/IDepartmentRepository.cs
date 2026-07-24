using Atlas.Application.Departments.Dtos;
using Atlas.Application.Matrix.Dtos;
using Atlas.Domain.Entities;

namespace Atlas.Application.Departments;

public interface IDepartmentRepository
{
    Task<IReadOnlyCollection<DepartmentListDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<DepartmentDetailDto?> GetDetailAsync(int id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<DepartmentToolLinkDto>> GetToolsAsync(int departmentId, CancellationToken cancellationToken);

    Task<DepartmentToolLinkDto?> GetToolLinkAsync(int departmentId, int toolId, CancellationToken cancellationToken);

    Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<MatrixLinkDto>> GetAllLinksAsync(CancellationToken cancellationToken);
}
