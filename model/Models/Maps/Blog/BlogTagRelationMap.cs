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
            entity.HasKey(p => p.BtrID);
            entity.Property(p => p.BtrID).HasColumnName("btr_id").UseMySqlIdentityColumn();
            entity.Property(p => p.BtrCreateTime).HasColumnName("btr_create_time");
            entity.Property(p => p.BtrIsDeleted).HasColumnName("btr_is_deleted");
            entity.Property(p => p.BtrTagName).HasColumnName("btr_tag_name");
            entity.Property(p => p.BtrUserID).HasColumnName("btr_user_id");
            entity.Property(p => p._blogIDList).HasColumnName("blog_idlist").IsRequired();
            entity.Ignore(p => p.BlogIDList);
            
        }
    }
}