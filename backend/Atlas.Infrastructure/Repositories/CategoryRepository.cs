using Atlas.Application.Categories;
using Atlas.Application.Categories.Dtos;
using Atlas.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Infrastructure.Repositories;

public sealed class CategoryRepository(AppDbContext dbContext, IMapper mapper) : ICategoryRepository
{
    public async Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken) =>
        await dbContext.Categories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Categories.AsNoTracking().AnyAsync(category => category.Id == id, cancellationToken);
}
