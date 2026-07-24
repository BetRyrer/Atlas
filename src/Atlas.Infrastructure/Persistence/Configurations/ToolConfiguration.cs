using System.Text.Json;
using Atlas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Infrastructure.Persistence.Configurations;

public sealed class ToolConfiguration : IEntityTypeConfiguration<Tool>
{
    public void Configure(EntityTypeBuilder<Tool> builder)
    {
        builder.ToTable("Tools");

        builder.HasKey(tool => tool.Id);

        builder.Property(tool => tool.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(tool => tool.Vendor)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(tool => tool.Version)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(tool => tool.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(tool => tool.DocumentationUrl)
            .HasMaxLength(500);

        builder.Property(tool => tool.LicenseType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(tool => tool.FoundedYear);

        builder.Property(tool => tool.LogoUrl)
            .HasMaxLength(500);

        builder.Property(tool => tool.YoutubeVideoUrl)
            .HasMaxLength(500);

        builder.Property<List<string>>("_availableVersions")
            .HasColumnName("AvailableVersions")
            .HasConversion(
                versions => JsonSerializer.Serialize(versions, (JsonSerializerOptions?)null),
                json => JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions?)null) ?? new List<string>())
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (left, right) => (left ?? new List<string>()).SequenceEqual(right ?? new List<string>()),
                versions => versions.Aggregate(0, (hash, version) => HashCode.Combine(hash, version.GetHashCode())),
                versions => versions.ToList()));

        builder.Property(tool => tool.CreatedAt).IsRequired();
        builder.Property(tool => tool.UpdatedAt).IsRequired();

        builder.HasIndex(tool => tool.Name);
        builder.HasIndex(tool => tool.CategoryId);

        builder.HasOne(tool => tool.Category)
            .WithMany(category => category.Tools)
            .HasForeignKey(tool => tool.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(tool => tool.DepartmentTools)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
