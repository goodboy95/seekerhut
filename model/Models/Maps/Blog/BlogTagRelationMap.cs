using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using System.Collections.Generic;

namespace Domain.Mapping
{
    public static class BlogTagRelationMap
    {
        public static void MapBlogTagRelation(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<BlogTagRelationEntity>();

            entity.ToTable("blog_tag_relation");
            entity.HasKey(p => p.ID);
            entity.Property(p => p.ID).HasColumnName("id").UseMySqlIdentityColumn();
            entity.Property(p => p.CreateTime).HasColumnName("create_time");
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted");
            entity.Property(p => p.TagName).HasColumnName("tag_name");
            entity.Property(p => p.UserID).HasColumnName("user_id");
            entity.Property(p => p._blogIDList).HasColumnName("blog_idlist").IsRequired();
            entity.Ignore(p => p.BlogIDList);
            
        }
    }
}