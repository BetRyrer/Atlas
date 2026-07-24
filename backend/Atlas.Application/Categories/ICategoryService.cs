using Atlas.Application.Categories.Dtos;

namespace Atlas.Application.Categories;

public interface ICategoryService
{
    Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken);
}
