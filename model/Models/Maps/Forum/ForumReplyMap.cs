using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Domain.Entity;
using System.Collections.Generic;
using System;

namespace Domain.Mapping
{
    public static class ForumReplyMap
    {
        public static void MapForumReply(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<ForumReplyEntity>();

            entity.ToTable("forum_reply");
            entity.HasKey(p => p.ID);
            entity.Property(p => p.ID).HasColumnName("id").UseMySqlIdentityColumn();
            entity.Property(p => p.CreateTime).HasColumnName("datetime");
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted");
            entity.Property(p => p.Author).HasColumnName("author").IsRequired();
            entity.Property(p => p.Content).HasColumnName("content").IsRequired();
            entity.Property(p => p.PostID).HasColumnName("post_id");
            entity.Property(p => p._subReplyID).HasColumnName("subreply_id").IsRequired();
            entity.Ignore(p => p.SubReplyID);
        }
    }
}