using Atlas.Application.Categories.Dtos;
using Atlas.Domain.Entities;
using AutoMapper;

namespace Atlas.Application.Categories;

public sealed class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}
