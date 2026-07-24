using Atlas.Domain.Enums;

namespace Atlas.Application.Tools.Dtos;

/// <summary>Fields shared by <see cref="CreateToolDto"/> and <see cref="UpdateToolDto"/>, factored out so both can share one validation rule set.</summary>
public interface IToolDto
{
    string Name { get; }
    string Vendor { get; }
    string Version { get; }
    string Description { get; }
    LicenseType LicenseType { get; }
    string? DocumentationUrl { get; }
    int CategoryId { get; }
    int? FoundedYear { get; }
    string? LogoUrl { get; }
    string? YoutubeVideoUrl { get; }
    IReadOnlyCollection<string> AvailableVersions { get; }
}
