using Atlas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Infrastructure.Persistence.Configurations;

public sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(department => department.Id);

        builder.Property(department => department.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(department => department.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(department => department.HeadCount)
            .IsRequired();

        builder.HasIndex(department => department.Name).IsUnique();

        builder.Navigation(department => department.DepartmentTools)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
