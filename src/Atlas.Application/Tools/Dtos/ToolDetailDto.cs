using Atlas.Domain.Enums;

namespace Atlas.Application.Tools.Dtos;

public sealed record ToolDetailDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Vendor { get; init; }
    public required string Version { get; init; }
    public required string Description { get; init; }
    public required LicenseType LicenseType { get; init; }
    public string? DocumentationUrl { get; init; }
    public required int CategoryId { get; init; }
    public required string CategoryName { get; init; }
    public int? FoundedYear { get; init; }
    public string? LogoUrl { get; init; }
    public string? YoutubeVideoUrl { get; init; }
    public required IReadOnlyCollection<string> AvailableVersions { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required IReadOnlyCollection<ToolDepartmentLinkDto> Departments { get; init; }
}
