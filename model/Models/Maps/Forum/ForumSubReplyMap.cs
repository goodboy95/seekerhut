using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Domain.Entity;
using System.Collections.Generic;
using System;

namespace Domain.Mapping
{
    public static class ForumSubReplyMap
    {
        public static void MapForumSubReply(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<ForumSubReplyEntity>();

            entity.ToTable("forum_subreply");
            entity.HasKey(p => p.ID);
            entity.Property(p => p.ID).HasColumnName("id").UseMySqlIdentityColumn();
            entity.Property(p => p.CreateTime).HasColumnName("datetime");
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted");
            entity.Property(p => p.ReplyID).HasColumnName("reply_id").IsRequired();
            entity.Property(p => p.Author).HasColumnName("author").IsRequired();
            entity.Property(p => p.Content).HasColumnName("content").IsRequired();
        }
    }
}