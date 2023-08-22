using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;

namespace WeeklyReview.Server.Persitance.Configurations
{
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
}
