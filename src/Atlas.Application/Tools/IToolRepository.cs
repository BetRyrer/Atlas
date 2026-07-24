using Atlas.Application.Common.Models;
using Atlas.Application.Tools.Dtos;
using Atlas.Domain.Entities;

namespace Atlas.Application.Tools;

public interface IToolRepository
{
    Task<PagedResult<ToolListDto>> GetPagedAsync(ToolQueryParameters parameters, CancellationToken cancellationToken);

    Task<ToolDetailDto?> GetDetailAsync(int id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<ToolListDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<Tool?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<Tool?> GetWithLinksByIdAsync(int id, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);

    void Add(Tool tool);

    void Remove(Tool tool);
}
