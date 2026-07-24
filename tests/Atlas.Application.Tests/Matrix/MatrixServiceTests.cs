using Atlas.Application.Departments;
using Atlas.Application.Departments.Dtos;
using Atlas.Application.Matrix;
using Atlas.Application.Matrix.Dtos;
using Atlas.Application.Tools;
using Atlas.Application.Tools.Dtos;
using Atlas.Domain.Enums;
using FluentAssertions;
using Moq;

namespace Atlas.Application.Tests.Matrix;

public sealed class MatrixServiceTests
{
    private readonly Mock<IDepartmentRepository> _departmentRepository = new();
    private readonly Mock<IToolRepository> _toolRepository = new();
    private readonly MatrixService _sut;

    public MatrixServiceTests()
    {
        _sut = new MatrixService(_departmentRepository.Object, _toolRepository.Object);
    }

    [Fact]
    public async Task GetMatrixAsync_WithPartialCoverage_BuildsGridWithNullCellsForMissingLinks()
    {
        IReadOnlyCollection<DepartmentListDto> departments =
        [
            new() { Id = 1, Name = "Développement Back-End", Description = "d", HeadCount = 12 },
            new() { Id = 2, Name = "Sécurité", Description = "d", HeadCount = 4 }
        ];

        IReadOnlyCollection<ToolListDto> tools =
        [
            new() { Id = 10, Name = "Git", Vendor = "v", Version = "1", LicenseType = LicenseType.OpenSource, CategoryId = 1, CategoryName = "Version Control" },
            new() { Id = 20, Name = "Vault", Vendor = "v", Version = "1", LicenseType = LicenseType.OpenSource, CategoryId = 5, CategoryName = "Containerization & Infrastructure" }
        ];

        IReadOnlyCollection<MatrixLinkDto> links =
        [
            new() { DepartmentId = 1, ToolId = 10, UsageLevel = UsageLevel.Primary },
            new() { DepartmentId = 2, ToolId = 20, UsageLevel = UsageLevel.Evaluating }
        ];

        _departmentRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(departments);
        _toolRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tools);
        _departmentRepository.Setup(repo => repo.GetAllLinksAsync(It.IsAny<CancellationToken>())).ReturnsAsync(links);

        var result = await _sut.GetMatrixAsync(CancellationToken.None);

        result.Tools.Should().HaveCount(2);
        result.Rows.Should().HaveCount(2);

        var backEndRow = result.Rows.Single(row => row.DepartmentId == 1);
        backEndRow.Cells.Single(cell => cell.ToolId == 10).UsageLevel.Should().Be(UsageLevel.Primary);
        backEndRow.Cells.Single(cell => cell.ToolId == 20).UsageLevel.Should().BeNull();

        var securityRow = result.Rows.Single(row => row.DepartmentId == 2);
        securityRow.Cells.Single(cell => cell.ToolId == 10).UsageLevel.Should().BeNull();
        securityRow.Cells.Single(cell => cell.ToolId == 20).UsageLevel.Should().Be(UsageLevel.Evaluating);
    }
}
