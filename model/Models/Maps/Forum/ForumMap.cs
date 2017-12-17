using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Domain.Entity;
using System.Collections.Generic;
using System;

namespace Domain.Mapping
{
    public static class ForumMap
    {
        public static void MapForum(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<ForumEntity>();

            entity.ToTable("forum");
            entity.HasKey(p => p.ForumID);
            entity.Property(p => p.ForumID).HasColumnName("forum_id").UseMySqlIdentityColumn();
            entity.Property(p => p.CreateTime).HasColumnName("datetime");
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted");
            entity.Property(p => p.Name).HasColumnName("name").IsRequired();
            entity.Property(p => p.CreatorID).HasColumnName("creator_id").IsRequired();
            entity.Property(p => p.Admin).HasColumnName("admin");
            entity.Property(p => p.PostLevel).HasColumnName("post_level");
            entity.Property(p => p.ViewLevel).HasColumnName("view_level");
            entity.Property(p => p.Status).HasColumnName("status");
            entity.Property(p => p._replyID).HasColumnName("reply_id").IsRequired();
            entity.Ignore(p => p.ReplyID);
        }
    }
}