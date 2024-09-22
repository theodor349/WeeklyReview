using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Drawing;

namespace Functions.Persitance.Configurations;

public class CategoryConfigurations : IEntityTypeConfiguration<CategoryModel>
{
    public void Configure(EntityTypeBuilder<CategoryModel> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(nameof(CategoryModel.UserGuid), nameof(CategoryModel.NormalizedName)).IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(64);

        builder.Property(x => x.NormalizedName)
            .HasMaxLength(64);

        builder.Property(x => x.Priority);

        builder.Property(x => x.UserGuid);

        builder.Property(x => x.Color)
            .HasConversion(
            v => v.ToArgb(),
            v => Color.FromArgb(v));
    }
}
