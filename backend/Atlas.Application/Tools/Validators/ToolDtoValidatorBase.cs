using Atlas.Application.Tools.Dtos;
using FluentValidation;

namespace Atlas.Application.Tools.Validators;

/// <summary>Validation rules shared by <see cref="CreateToolDto"/> and <see cref="UpdateToolDto"/> — both carry the same fields, only the DTO identity differs by intent.</summary>
public abstract class ToolDtoValidatorBase<T> : AbstractValidator<T> where T : IToolDto
{
    protected ToolDtoValidatorBase()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Vendor).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Version).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.LicenseType).IsInEnum();
        RuleFor(x => x.DocumentationUrl).Must(BeAValidUrl).When(x => x.DocumentationUrl is not null)
            .WithMessage("DocumentationUrl must be a valid absolute URL.");
        RuleFor(x => x.CategoryId).GreaterThan(0);
        RuleFor(x => x.FoundedYear)
            .InclusiveBetween(1970, DateTime.UtcNow.Year)
            .When(x => x.FoundedYear is not null);
        RuleFor(x => x.LogoUrl).Must(BeAValidUrl).When(x => x.LogoUrl is not null)
            .WithMessage("LogoUrl must be a valid absolute URL.");
        RuleFor(x => x.YoutubeVideoUrl).Must(BeAYoutubeUrl).When(x => x.YoutubeVideoUrl is not null)
            .WithMessage("YoutubeVideoUrl must be a valid youtube.com or youtu.be URL.");
        RuleForEach(x => x.AvailableVersions).NotEmpty().MaximumLength(50);
    }

    private static bool BeAValidUrl(string? url) =>
        Uri.TryCreate(url, UriKind.Absolute, out _);

    private static bool BeAYoutubeUrl(string? url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var uri)
        && (uri.Host.EndsWith("youtube.com", StringComparison.OrdinalIgnoreCase)
            || uri.Host.EndsWith("youtu.be", StringComparison.OrdinalIgnoreCase));
}
