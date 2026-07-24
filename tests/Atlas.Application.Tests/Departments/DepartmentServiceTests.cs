using Atlas.Application.Common;
using Atlas.Application.Common.Exceptions;
using Atlas.Application.Departments;
using Atlas.Application.Departments.Dtos;
using Atlas.Application.Tests.TestSupport;
using Atlas.Application.Tools;
using Atlas.Domain.Entities;
using Atlas.Domain.Enums;
using FluentAssertions;
using Moq;

namespace Atlas.Application.Tests.Departments;

public sealed class DepartmentServiceTests
{
    private readonly Mock<IDepartmentRepository> _departmentRepository = new();
    private readonly Mock<IToolRepository> _toolRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly DepartmentService _sut;

    public DepartmentServiceTests()
    {
        _sut = new DepartmentService(_departmentRepository.Object, _toolRepository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ThrowsNotFoundException()
    {
        _departmentRepository.Setup(repo => repo.GetDetailAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((DepartmentDetailDto?)null);

        var act = () => _sut.GetByIdAsync(99, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task LinkToolAsync_NewLink_LinksToolAndReturnsDto()
    {
        var department = new Department("Sécurité", "Audit et conformité.", 4).WithId(6);
        var tool = new Tool("Vault", "HashiCorp", "1.0", "Secrets manager.", LicenseType.OpenSource, null, 5).WithId(41);

        _departmentRepository.Setup(repo => repo.GetByIdAsync(6, It.IsAny<CancellationToken>())).ReturnsAsync(department);
        _toolRepository.Setup(repo => repo.GetWithLinksByIdAsync(41, It.IsAny<CancellationToken>())).ReturnsAsync(tool);

        var expectedDto = new DepartmentToolLinkDto
        {
            ToolId = 41,
            ToolName = "Vault",
            CategoryName = "Containerization & Infrastructure",
            UsageLevel = UsageLevel.Evaluating,
            Referent = "Antoine Rousseau",
            AdoptedOn = null
        };
        _departmentRepository.Setup(repo => repo.GetToolLinkAsync(6, 41, It.IsAny<CancellationToken>())).ReturnsAsync(expectedDto);

        var dto = new LinkToolDto { ToolId = 41, UsageLevel = UsageLevel.Evaluating, Referent = "Antoine Rousseau" };

        var result = await _sut.LinkToolAsync(6, dto, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedDto);
        tool.DepartmentTools.Should().ContainSingle(link => link.DepartmentId == 6 && link.ToolId == 41);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LinkToolAsync_AlreadyLinked_ThrowsConflictExceptionAndDoesNotPersist()
    {
        var department = new Department("Sécurité", "Audit et conformité.", 4).WithId(6);
        var tool = new Tool("Vault", "HashiCorp", "1.0", "Secrets manager.", LicenseType.OpenSource, null, 5).WithId(41);
        tool.LinkTo(department, UsageLevel.Primary);

        _departmentRepository.Setup(repo => repo.GetByIdAsync(6, It.IsAny<CancellationToken>())).ReturnsAsync(department);
        _toolRepository.Setup(repo => repo.GetWithLinksByIdAsync(41, It.IsAny<CancellationToken>())).ReturnsAsync(tool);

        var dto = new LinkToolDto { ToolId = 41, UsageLevel = UsageLevel.Secondary };

        var act = () => _sut.LinkToolAsync(6, dto, CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>();
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task LinkToolAsync_NonExistingDepartment_ThrowsNotFoundException()
    {
        _departmentRepository.Setup(repo => repo.GetByIdAsync(6, It.IsAny<CancellationToken>())).ReturnsAsync((Department?)null);

        var dto = new LinkToolDto { ToolId = 41, UsageLevel = UsageLevel.Primary };

        var act = () => _sut.LinkToolAsync(6, dto, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UnlinkToolAsync_NotLinked_ThrowsNotFoundException()
    {
        var tool = new Tool("Vault", "HashiCorp", "1.0", "Secrets manager.", LicenseType.OpenSource, null, 5).WithId(41);

        _departmentRepository.Setup(repo => repo.ExistsAsync(6, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _toolRepository.Setup(repo => repo.GetWithLinksByIdAsync(41, It.IsAny<CancellationToken>())).ReturnsAsync(tool);

        var act = () => _sut.UnlinkToolAsync(6, 41, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UnlinkToolAsync_ExistingLink_RemovesLinkAndPersists()
    {
        var department = new Department("Sécurité", "Audit et conformité.", 4).WithId(6);
        var tool = new Tool("Vault", "HashiCorp", "1.0", "Secrets manager.", LicenseType.OpenSource, null, 5).WithId(41);
        tool.LinkTo(department, UsageLevel.Primary);

        _departmentRepository.Setup(repo => repo.ExistsAsync(6, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _toolRepository.Setup(repo => repo.GetWithLinksByIdAsync(41, It.IsAny<CancellationToken>())).ReturnsAsync(tool);

        await _sut.UnlinkToolAsync(6, 41, CancellationToken.None);

        tool.DepartmentTools.Should().BeEmpty();
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
