﻿using CommunityItaly.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityItaly.EF.EntityConfigurations
{
	public class ArticleEntityBuilder : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToContainer(nameof(Article))
               .HasNoDiscriminator()
               .HasPartitionKey(o => o.Id);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PublishDate);
            builder.Property(x => x.Url);
            builder.Property(x => x.Name);
            //builder.HasMany(x => x.Authors);
        }
    }
}
