using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WeeklyReview.Database.Models;

namespace WeeklyReview.Server.Persitance.Configurations
{
    public class ActivityChangeConfigurations : IEntityTypeConfiguration<ActivityChangeModel>
    {
        public void Configure(EntityTypeBuilder<ActivityChangeModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ChangeDate);

            builder.HasOne(x => x.Source);

            builder.HasOne(x => x.Destination);
        }
    }
}
