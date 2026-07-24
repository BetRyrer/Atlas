using Atlas.Application.Categories;
using Atlas.Application.Common;
using Atlas.Application.Common.Exceptions;
using Atlas.Application.Common.Models;
using Atlas.Application.Tools;
using Atlas.Application.Tools.Dtos;
using Atlas.Domain.Entities;
using Atlas.Domain.Enums;
using FluentAssertions;
using Moq;

namespace Atlas.Application.Tests.Tools;

public sealed class ToolServiceTests
{
    private readonly Mock<IToolRepository> _toolRepository = new();
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly ToolService _sut;

    public ToolServiceTests()
    {
        _sut = new ToolService(_toolRepository.Object, _categoryRepository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task GetPagedAsync_WithSearchAndCategoryFilters_ReturnsFilteredPagedResult()
    {
        var query = new ToolQueryParameters { Search = "git", CategoryId = 2, LicenseType = LicenseType.OpenSource, Page = 1, PageSize = 10 };
        var expected = new PagedResult<ToolListDto>
        {
            Items = [new ToolListDto { Id = 1, Name = "Git", Vendor = "Software Freedom Conservancy", Version = "2.45", LicenseType = LicenseType.OpenSource, CategoryId = 2, CategoryName = "Version Control" }],
            TotalCount = 1,
            Page = 1,
            PageSize = 10
        };
        _toolRepository.Setup(repo => repo.GetPagedAsync(query, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _sut.GetPagedAsync(query, CancellationToken.None);

        result.Should().BeEquivalentTo(expected);
        _toolRepository.Verify(repo => repo.GetPagedAsync(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ThrowsNotFoundException()
    {
        _toolRepository.Setup(repo => repo.GetDetailAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((ToolDetailDto?)null);

        var act = () => _sut.GetByIdAsync(99, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_UnknownCategory_ThrowsNotFoundExceptionAndDoesNotPersist()
    {
        var dto = new CreateToolDto { Name = "Linear", Vendor = "Linear Inc.", Version = "1.0", Description = "Issue tracking.", LicenseType = LicenseType.Freemium, CategoryId = 42 };
        _categoryRepository.Setup(repo => repo.ExistsAsync(42, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var act = () => _sut.CreateAsync(dto, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _toolRepository.Verify(repo => repo.Add(It.IsAny<Tool>()), Times.Never);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_AddsToolAndPersistsChanges()
    {
        var dto = new CreateToolDto { Name = "Linear", Vendor = "Linear Inc.", Version = "1.0", Description = "Issue tracking.", LicenseType = LicenseType.Freemium, CategoryId = 3 };
        _categoryRepository.Setup(repo => repo.ExistsAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _toolRepository.Setup(repo => repo.GetDetailAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ToolDetailDto
            {
                Id = 0,
                Name = "Linear",
                Vendor = "Linear Inc.",
                Version = "1.0",
                Description = "Issue tracking.",
                LicenseType = LicenseType.Freemium,
                CategoryId = 3,
                CategoryName = "Project Management",
                AvailableVersions = [],
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Departments = []
            });

        var result = await _sut.CreateAsync(dto, CancellationToken.None);

        _toolRepository.Verify(repo => repo.Add(It.Is<Tool>(tool => tool.Name == "Linear")), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Name.Should().Be("Linear");
    }

    [Fact]
    public async Task UpdateAsync_NonExistingTool_ThrowsNotFoundException()
    {
        _toolRepository.Setup(repo => repo.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync((Tool?)null);
        var dto = new UpdateToolDto { Name = "X", Vendor = "Y", Version = "1", Description = "d", LicenseType = LicenseType.Internal, CategoryId = 1 };

        var act = () => _sut.UpdateAsync(5, dto, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_NonExistingTool_ThrowsNotFoundException()
    {
        _toolRepository.Setup(repo => repo.GetByIdAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync((Tool?)null);

        var act = () => _sut.DeleteAsync(7, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _toolRepository.Verify(repo => repo.Remove(It.IsAny<Tool>()), Times.Never);
    }
}
