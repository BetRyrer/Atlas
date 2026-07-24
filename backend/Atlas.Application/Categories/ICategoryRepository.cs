using Atlas.Application.Categories.Dtos;

namespace Atlas.Application.Categories;

public interface ICategoryRepository
{
    Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
}
