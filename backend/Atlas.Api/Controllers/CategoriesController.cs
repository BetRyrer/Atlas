using Atlas.Application.Categories;
using Atlas.Application.Categories.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.Api.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoriesController(ICategoryRepository categoryRepository) : ControllerBase
{
    /// <summary>Returns every category.</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CategoryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetAllAsync(cancellationToken);
        return Ok(categories);
    }
}
