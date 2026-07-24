using Atlas.Application.Common;
using Atlas.Application.Common.Exceptions;
using Atlas.Application.Departments.Dtos;
using Atlas.Application.Tools;
using Atlas.Domain.Entities;

namespace Atlas.Application.Departments;

public sealed class DepartmentService(
    IDepartmentRepository departmentRepository,
    IToolRepository toolRepository,
    IUnitOfWork unitOfWork) : IDepartmentService
{
    public Task<IReadOnlyCollection<DepartmentListDto>> GetAllAsync(CancellationToken cancellationToken) =>
        departmentRepository.GetAllAsync(cancellationToken);

    public async Task<DepartmentDetailDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var dto = await departmentRepository.GetDetailAsync(id, cancellationToken);
        return dto ?? throw NotFoundException.ForEntity(nameof(Department), id);
    }

    public async Task<IReadOnlyCollection<DepartmentToolLinkDto>> GetToolsAsync(int departmentId, CancellationToken cancellationToken)
    {
        if (!await departmentRepository.ExistsAsync(departmentId, cancellationToken))
        {
            throw NotFoundException.ForEntity(nameof(Department), departmentId);
        }

        return await departmentRepository.GetToolsAsync(departmentId, cancellationToken);
    }

    public async Task<DepartmentToolLinkDto> LinkToolAsync(int departmentId, LinkToolDto dto, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetByIdAsync(departmentId, cancellationToken)
            ?? throw NotFoundException.ForEntity(nameof(Department), departmentId);

        var tool = await toolRepository.GetWithLinksByIdAsync(dto.ToolId, cancellationToken)
            ?? throw NotFoundException.ForEntity(nameof(Tool), dto.ToolId);

        if (tool.DepartmentTools.Any(link => link.DepartmentId == departmentId))
        {
            throw new ConflictException($"Tool '{tool.Name}' is already linked to department '{department.Name}'.");
        }

        tool.LinkTo(department, dto.UsageLevel, dto.Referent, dto.AdoptedOn);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await departmentRepository.GetToolLinkAsync(departmentId, dto.ToolId, cancellationToken)
            ?? throw NotFoundException.ForEntity(nameof(Tool), dto.ToolId);
    }

    public async Task UnlinkToolAsync(int departmentId, int toolId, CancellationToken cancellationToken)
    {
        if (!await departmentRepository.ExistsAsync(departmentId, cancellationToken))
        {
            throw NotFoundException.ForEntity(nameof(Department), departmentId);
        }

        var tool = await toolRepository.GetWithLinksByIdAsync(toolId, cancellationToken)
            ?? throw NotFoundException.ForEntity(nameof(Tool), toolId);

        if (tool.DepartmentTools.All(link => link.DepartmentId != departmentId))
        {
            throw NotFoundException.ForEntity("DepartmentTool link", $"{departmentId}/{toolId}");
        }

        tool.Unlink(departmentId);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
