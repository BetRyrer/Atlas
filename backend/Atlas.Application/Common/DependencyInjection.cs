using System.Reflection;
using Atlas.Application.Auth;
using Atlas.Application.Departments;
using Atlas.Application.Matrix;
using Atlas.Application.Tools;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Application.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddAutoMapper(_ => { }, assembly);
        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<IToolService, ToolService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IMatrixService, MatrixService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
