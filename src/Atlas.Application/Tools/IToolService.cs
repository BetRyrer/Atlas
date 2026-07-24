using Atlas.Application.Common.Models;
using Atlas.Application.Tools.Dtos;

namespace Atlas.Application.Tools;

public interface IToolService
{
    Task<PagedResult<ToolListDto>> GetPagedAsync(ToolQueryParameters parameters, CancellationToken cancellationToken);

    Task<ToolDetailDto> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<ToolDetailDto> CreateAsync(CreateToolDto dto, CancellationToken cancellationToken);

    Task<ToolDetailDto> UpdateAsync(int id, UpdateToolDto dto, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
