using Atlas.Application.Categories.Dtos;

namespace Atlas.Application.Categories;

public sealed class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken) =>
        categoryRepository.GetAllAsync(cancellationToken);
}
