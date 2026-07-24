using Atlas.Application.Departments.Dtos;
using Atlas.Domain.Entities;
using AutoMapper;

namespace Atlas.Application.Departments;

public sealed class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        CreateMap<Department, DepartmentListDto>()
            .ForMember(dest => dest.ToolCount, opt => opt.MapFrom(src => src.DepartmentTools.Count));

        CreateMap<Department, DepartmentDetailDto>()
            .ForMember(dest => dest.Tools, opt => opt.MapFrom(src => src.DepartmentTools));

        CreateMap<DepartmentTool, DepartmentToolLinkDto>()
            .ForMember(dest => dest.ToolName, opt => opt.MapFrom(src => src.Tool.Name))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Tool.Category.Name))
            .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.Tool.LogoUrl));
    }
}
