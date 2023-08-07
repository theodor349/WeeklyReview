using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WeeklyReview.Database.Models;

namespace WeeklyReview.Server.Persitance.Configurations
{
    public class ActivityConfigurations : IEntityTypeConfiguration<ActivityModel>
    {
        public void Configure(EntityTypeBuilder<ActivityModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name)
                .HasMaxLength(64);

            builder.Property(x => x.NormalizedName)
                .HasMaxLength(64);

            builder.Property(x => x.Deleted);

            builder.HasOne(x => x.Category);
        }
    }
}
