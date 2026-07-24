using Atlas.Application.Tools.Dtos;
using FluentValidation;

namespace Atlas.Application.Tools.Validators;

public sealed class ToolQueryParametersValidator : AbstractValidator<ToolQueryParameters>
{
    public ToolQueryParametersValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.CategoryId).GreaterThan(0).When(x => x.CategoryId is not null);
        RuleFor(x => x.LicenseType).IsInEnum().When(x => x.LicenseType is not null);
        RuleFor(x => x.SortBy).IsInEnum();
    }
}
