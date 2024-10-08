﻿using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Functions.Persitance.Configurations;

public class ActivityChangeConfigurations : IEntityTypeConfiguration<ActivityChangeModel>
{
    public void Configure(EntityTypeBuilder<ActivityChangeModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.ChangeDate);

        builder.Property(x => x.UserGuid);

        builder.HasOne(x => x.Source).WithMany().OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Destination).WithMany().OnDelete(DeleteBehavior.NoAction);
    }
}
