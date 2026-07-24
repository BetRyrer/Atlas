namespace Atlas.Application.Departments.Dtos;

public sealed record DepartmentDetailDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int HeadCount { get; init; }
    public required IReadOnlyCollection<DepartmentToolLinkDto> Tools { get; init; }
}
