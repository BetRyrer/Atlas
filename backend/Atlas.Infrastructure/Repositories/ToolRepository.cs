using Atlas.Application.Common.Models;
using Atlas.Application.Tools;
using Atlas.Application.Tools.Dtos;
using Atlas.Domain.Entities;
using Atlas.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Infrastructure.Repositories;

public sealed class ToolRepository(AppDbContext dbContext, IMapper mapper) : IToolRepository
{
    public async Task<PagedResult<ToolListDto>> GetPagedAsync(ToolQueryParameters parameters, CancellationToken cancellationToken)
    {
        var query = dbContext.Tools.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            var pattern = $"%{parameters.Search.Trim()}%";
            query = query.Where(tool => EF.Functions.Like(tool.Name, pattern) || EF.Functions.Like(tool.Vendor, pattern));
        }

        if (parameters.CategoryId is { } categoryId)
        {
            query = query.Where(tool => tool.CategoryId == categoryId);
        }

        if (parameters.LicenseType is { } licenseType)
        {
            query = query.Where(tool => tool.LicenseType == licenseType);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = ApplySort(query, parameters.SortBy, parameters.Descending);

        var items = await query
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ProjectTo<ToolListDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PagedResult<ToolListDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = parameters.Page,
            PageSize = parameters.PageSize
        };
    }

    public async Task<ToolDetailDto?> GetDetailAsync(int id, CancellationToken cancellationToken) =>
        await dbContext.Tools
            .AsNoTracking()
            .Where(tool => tool.Id == id)
            .ProjectTo<ToolDetailDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyCollection<ToolListDto>> GetAllAsync(CancellationToken cancellationToken) =>
        await dbContext.Tools
            .AsNoTracking()
            .OrderBy(tool => tool.Name)
            .ProjectTo<ToolListDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

    public Task<Tool?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Tools.FirstOrDefaultAsync(tool => tool.Id == id, cancellationToken);

    public Task<Tool?> GetWithLinksByIdAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Tools
            .Include(tool => tool.DepartmentTools)
            .FirstOrDefaultAsync(tool => tool.Id == id, cancellationToken);

    public void Add(Tool tool) => dbContext.Tools.Add(tool);

    public void Remove(Tool tool) => dbContext.Tools.Remove(tool);

    private static IQueryable<Tool> ApplySort(IQueryable<Tool> query, ToolSortColumn sortBy, bool descending) => sortBy switch
    {
        ToolSortColumn.Vendor => descending ? query.OrderByDescending(tool => tool.Vendor) : query.OrderBy(tool => tool.Vendor),
        ToolSortColumn.Version => descending ? query.OrderByDescending(tool => tool.Version) : query.OrderBy(tool => tool.Version),
        ToolSortColumn.Category => descending ? query.OrderByDescending(tool => tool.Category.Name) : query.OrderBy(tool => tool.Category.Name),
        ToolSortColumn.License => descending ? query.OrderByDescending(tool => tool.LicenseType) : query.OrderBy(tool => tool.LicenseType),
        _ => descending ? query.OrderByDescending(tool => tool.Name) : query.OrderBy(tool => tool.Name),
    };
}
