using Atlas.Domain.Enums;

namespace Atlas.Domain.Entities;

public sealed class Tool
{
    private readonly List<DepartmentTool> _departmentTools = [];
    private readonly List<string> _availableVersions = [];

    private Tool()
    {
    }

    public Tool(
        string name,
        string vendor,
        string version,
        string description,
        LicenseType licenseType,
        string? documentationUrl,
        int categoryId,
        int? foundedYear = null,
        string? logoUrl = null,
        IEnumerable<string>? availableVersions = null,
        string? youtubeVideoUrl = null)
    {
        Name = name;
        Vendor = vendor;
        Version = version;
        Description = description;
        LicenseType = licenseType;
        DocumentationUrl = documentationUrl;
        CategoryId = categoryId;
        FoundedYear = foundedYear;
        LogoUrl = logoUrl;
        YoutubeVideoUrl = youtubeVideoUrl;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;

        if (availableVersions is not null)
        {
            _availableVersions.AddRange(availableVersions);
        }
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Vendor { get; private set; } = string.Empty;
    public string Version { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public LicenseType LicenseType { get; private set; }
    public string? DocumentationUrl { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public int? FoundedYear { get; private set; }
    public string? LogoUrl { get; private set; }
    public string? YoutubeVideoUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public IReadOnlyCollection<DepartmentTool> DepartmentTools => _departmentTools.AsReadOnly();
    public IReadOnlyCollection<string> AvailableVersions => _availableVersions.AsReadOnly();

    public void UpdateDetails(
        string name,
        string vendor,
        string description,
        LicenseType licenseType,
        string? documentationUrl,
        int categoryId,
        int? foundedYear,
        string? logoUrl,
        string? youtubeVideoUrl)
    {
        Name = name;
        Vendor = vendor;
        Description = description;
        LicenseType = licenseType;
        DocumentationUrl = documentationUrl;
        CategoryId = categoryId;
        FoundedYear = foundedYear;
        LogoUrl = logoUrl;
        YoutubeVideoUrl = youtubeVideoUrl;
        Touch();
    }

    public void UpdateVersion(string version)
    {
        Version = version;
        Touch();
    }

    public void UpdateAvailableVersions(IEnumerable<string> availableVersions)
    {
        _availableVersions.Clear();
        _availableVersions.AddRange(availableVersions);
        Touch();
    }

    public DepartmentTool LinkTo(Department department, UsageLevel usageLevel, string? referent = null, DateOnly? adoptedOn = null)
    {
        var link = new DepartmentTool(department.Id, Id, usageLevel, referent, adoptedOn);
        _departmentTools.Add(link);
        Touch();
        return link;
    }

    public void Unlink(int departmentId)
    {
        var link = _departmentTools.FirstOrDefault(dt => dt.DepartmentId == departmentId);
        if (link is not null)
        {
            _departmentTools.Remove(link);
            Touch();
        }
    }

    private void Touch() => UpdatedAt = DateTime.UtcNow;
}
