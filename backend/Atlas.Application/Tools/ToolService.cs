using Atlas.Application.Categories;
using Atlas.Application.Common;
using Atlas.Application.Common.Exceptions;
using Atlas.Application.Common.Models;
using Atlas.Application.Tools.Dtos;
using Atlas.Domain.Entities;

namespace Atlas.Application.Tools;

public sealed class ToolService(
    IToolRepository toolRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : IToolService
{
    public Task<PagedResult<ToolListDto>> GetPagedAsync(ToolQueryParameters parameters, CancellationToken cancellationToken) =>
        toolRepository.GetPagedAsync(parameters, cancellationToken);

    public async Task<ToolDetailDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var dto = await toolRepository.GetDetailAsync(id, cancellationToken);
        return dto ?? throw NotFoundException.ForEntity(nameof(Tool), id);
    }

    public async Task<ToolDetailDto> CreateAsync(CreateToolDto dto, CancellationToken cancellationToken)
    {
        if (!await categoryRepository.ExistsAsync(dto.CategoryId, cancellationToken))
        {
            throw NotFoundException.ForEntity("Category", dto.CategoryId);
        }

        var tool = new Tool(
            dto.Name,
            dto.Vendor,
            dto.Version,
            dto.Description,
            dto.LicenseType,
            dto.DocumentationUrl,
            dto.CategoryId,
            dto.FoundedYear,
            dto.LogoUrl,
            dto.AvailableVersions,
            dto.YoutubeVideoUrl);
        toolRepository.Add(tool);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(tool.Id, cancellationToken);
    }

    public async Task<ToolDetailDto> UpdateAsync(int id, UpdateToolDto dto, CancellationToken cancellationToken)
    {
        var tool = await toolRepository.GetByIdAsync(id, cancellationToken)
            ?? throw NotFoundException.ForEntity(nameof(Tool), id);

        if (!await categoryRepository.ExistsAsync(dto.CategoryId, cancellationToken))
        {
            throw NotFoundException.ForEntity("Category", dto.CategoryId);
        }

        tool.UpdateDetails(
            dto.Name,
            dto.Vendor,
            dto.Description,
            dto.LicenseType,
            dto.DocumentationUrl,
            dto.CategoryId,
            dto.FoundedYear,
            dto.LogoUrl,
            dto.YoutubeVideoUrl);
        tool.UpdateVersion(dto.Version);
        tool.UpdateAvailableVersions(dto.AvailableVersions);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(tool.Id, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var tool = await toolRepository.GetByIdAsync(id, cancellationToken)
            ?? throw NotFoundException.ForEntity(nameof(Tool), id);

        toolRepository.Remove(tool);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
