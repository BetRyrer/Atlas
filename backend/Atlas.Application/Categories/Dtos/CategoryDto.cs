namespace Atlas.Application.Categories.Dtos;

public sealed record CategoryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
}
