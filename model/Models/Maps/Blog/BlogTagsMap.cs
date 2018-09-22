using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using System.Collections.Generic;

namespace Domain.Mapping
{
    public static class BlogTagsMap
    {
        public static void MapBlogTags(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<BlogTagsEntity>();

            entity.ToTable("blog_tags");
            entity.HasKey(p => p.ID);
            entity.Property(p => p.ID).HasColumnName("id").UseMySqlIdentityColumn();
            entity.Property(p => p.CreateTime).HasColumnName("create_time");
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted");
            entity.Property(p => p.TagName).HasColumnName("tag_name");
            entity.Property(p => p.UserID).HasColumnName("user_id");
            
        }
    }
}