using database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Functions.Persitance.Configurations;

public class ActivityConfigurations : IEntityTypeConfiguration<ActivityModel>
{
    public void Configure(EntityTypeBuilder<ActivityModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.HasIndex(nameof(ActivityModel.UserGuid), nameof(ActivityModel.NormalizedName)).IsUnique();
        builder.Property(x => x.Name)
            .HasMaxLength(128);

        builder.Property(x => x.NormalizedName)
            .HasMaxLength(128);

        builder.Property(x => x.UserGuid);

        builder.Property(x => x.Deleted);

        builder.HasOne(x => x.Category);
    }
}
