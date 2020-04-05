﻿using CommunityItaly.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityItaly.EF.EntityConfigurations
{
	public class CommunityEntityBuilder : IEntityTypeConfiguration<Community>
    {
        public void Configure(EntityTypeBuilder<Community> builder)
        {
            builder.ToContainer(nameof(Community))
                .HasNoDiscriminator();
            builder.HasKey(x => x.ShortName);


            builder.Property(x => x.ShortName);
            builder.HasMany(x => x.Managers);
        }
    }
}
