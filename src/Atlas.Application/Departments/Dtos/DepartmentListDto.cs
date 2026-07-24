namespace Atlas.Application.Departments.Dtos;

public sealed record DepartmentListDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int HeadCount { get; init; }
}
