using Atlas.Application.Departments.Dtos;
using FluentValidation;

namespace Atlas.Application.Departments.Validators;

public sealed class LinkToolDtoValidator : AbstractValidator<LinkToolDto>
{
    public LinkToolDtoValidator()
    {
        RuleFor(x => x.ToolId).GreaterThan(0);
        RuleFor(x => x.UsageLevel).IsInEnum();
        RuleFor(x => x.Referent).MaximumLength(100);
        RuleFor(x => x.AdoptedOn)
            .Must(adoptedOn => adoptedOn is null || adoptedOn <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("AdoptedOn cannot be in the future.");
    }
}
