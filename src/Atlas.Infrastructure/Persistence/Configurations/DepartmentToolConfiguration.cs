using Atlas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Infrastructure.Persistence.Configurations;

public sealed class DepartmentToolConfiguration : IEntityTypeConfiguration<DepartmentTool>
{
    public void Configure(EntityTypeBuilder<DepartmentTool> builder)
    {
        builder.ToTable("DepartmentTools");

        builder.HasKey(link => new { link.DepartmentId, link.ToolId });

        builder.Property(link => link.UsageLevel)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(link => link.Referent)
            .HasMaxLength(100);

        builder.HasOne(link => link.Department)
            .WithMany(department => department.DepartmentTools)
            .HasForeignKey(link => link.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(link => link.Tool)
            .WithMany(tool => tool.DepartmentTools)
            .HasForeignKey(link => link.ToolId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
