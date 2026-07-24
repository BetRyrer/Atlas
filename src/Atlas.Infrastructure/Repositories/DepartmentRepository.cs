using Atlas.Application.Departments;
using Atlas.Application.Departments.Dtos;
using Atlas.Application.Matrix.Dtos;
using Atlas.Domain.Entities;
using Atlas.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Infrastructure.Repositories;

public sealed class DepartmentRepository(AppDbContext dbContext, IMapper mapper) : IDepartmentRepository
{
    public async Task<IReadOnlyCollection<DepartmentListDto>> GetAllAsync(CancellationToken cancellationToken) =>
        await dbContext.Departments
            .AsNoTracking()
            .OrderBy(department => department.Name)
            .ProjectTo<DepartmentListDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

    public async Task<DepartmentDetailDto?> GetDetailAsync(int id, CancellationToken cancellationToken) =>
        await dbContext.Departments
            .AsNoTracking()
            .Where(department => department.Id == id)
            .ProjectTo<DepartmentDetailDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyCollection<DepartmentToolLinkDto>> GetToolsAsync(int departmentId, CancellationToken cancellationToken) =>
        await dbContext.DepartmentTools
            .AsNoTracking()
            .Where(link => link.DepartmentId == departmentId)
            .OrderBy(link => link.Tool.Name)
            .ProjectTo<DepartmentToolLinkDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

    public async Task<DepartmentToolLinkDto?> GetToolLinkAsync(int departmentId, int toolId, CancellationToken cancellationToken) =>
        await dbContext.DepartmentTools
            .AsNoTracking()
            .Where(link => link.DepartmentId == departmentId && link.ToolId == toolId)
            .ProjectTo<DepartmentToolLinkDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Departments.FirstOrDefaultAsync(department => department.Id == id, cancellationToken);

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Departments.AsNoTracking().AnyAsync(department => department.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<MatrixLinkDto>> GetAllLinksAsync(CancellationToken cancellationToken) =>
        await dbContext.DepartmentTools
            .AsNoTracking()
            .ProjectTo<MatrixLinkDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
}
