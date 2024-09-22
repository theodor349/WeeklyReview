using database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Functions.Persitance.Configurations;

public class EntryConfigurations : IEntityTypeConfiguration<EntryModel>
{
    public void Configure(EntityTypeBuilder<EntryModel> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.StartTime);

        builder.Property(x => x.EndTime);

        builder.Property(x => x.Deleted);

        builder.Property(x => x.UserGuid);

        builder.HasMany(x => x.Activities)
            .WithMany();
    }
}
