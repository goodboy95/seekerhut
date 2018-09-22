using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using System.Collections.Generic;

namespace Domain.Mapping
{
    public static class BlogTagRelationMap
    {
        public static void MapBlogTagRelation(this ModelBuilder builder)
        {
            var entity = builder.Entity<BlogTagRelationEntity>();

            entity.ToTable("blog_tag_relation");
            entity.HasKey(p => p.ID);
            entity.Property(p => p.ID).HasColumnName("id").UseMySqlIdentityColumn();
            entity.Property(p => p.CreateTime).HasColumnName("create_time");
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted");
            entity.Property(p => p.TagId).HasColumnName("tag_id").IsRequired();
            entity.Property(p => p.BlogId).HasColumnName("blog_id").IsRequired();
            entity.HasIndex(p => p.TagId);
            entity.HasIndex(p => p.BlogId);
        }
    }
}