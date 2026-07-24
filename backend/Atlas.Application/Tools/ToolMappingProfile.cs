using Atlas.Application.Tools.Dtos;
using Atlas.Domain.Entities;
using AutoMapper;

namespace Atlas.Application.Tools;

public sealed class ToolMappingProfile : Profile
{
    public ToolMappingProfile()
    {
        CreateMap<Tool, ToolListDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<Tool, ToolDetailDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Departments, opt => opt.MapFrom(src => src.DepartmentTools));

        CreateMap<DepartmentTool, ToolDepartmentLinkDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));
    }
}
