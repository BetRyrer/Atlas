using Atlas.Domain.Enums;

namespace Atlas.Application.Tools.Dtos;

public sealed record ToolQueryParameters
{
    public string? Search { get; init; }
    public int? CategoryId { get; init; }
    public LicenseType? LicenseType { get; init; }
    public ToolSortColumn SortBy { get; init; } = ToolSortColumn.Name;
    public bool Descending { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
