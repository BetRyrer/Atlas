using Atlas.Domain.Enums;

namespace Atlas.Application.Tools.Dtos;

public sealed record ToolListDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Vendor { get; init; }
    public required string Version { get; init; }
    public required LicenseType LicenseType { get; init; }
    public required int CategoryId { get; init; }
    public required string CategoryName { get; init; }
    public string? LogoUrl { get; init; }
}
