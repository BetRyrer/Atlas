using Atlas.Application.Matrix.Dtos;
using Atlas.Domain.Entities;
using AutoMapper;

namespace Atlas.Application.Matrix;

public sealed class MatrixMappingProfile : Profile
{
    public MatrixMappingProfile()
    {
        CreateMap<DepartmentTool, MatrixLinkDto>();
    }
}
