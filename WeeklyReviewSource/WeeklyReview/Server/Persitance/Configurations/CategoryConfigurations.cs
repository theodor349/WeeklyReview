using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WeeklyReview.Database.Models;
using System.Drawing;

namespace WeeklyReview.Server.Persitance.Configurations
{
    public class CategoryConfigurations : IEntityTypeConfiguration<CategoryModel>
    {
        public void Configure(EntityTypeBuilder<CategoryModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(64);

            builder.Property(x => x.NormalizedName)
                .HasMaxLength(64);

            builder.Property(x => x.Priority);

            builder.Property(x => x.Color)
                .HasConversion(
                v => v.ToArgb(),
                v => Color.FromArgb(v));
        }
    }
}
