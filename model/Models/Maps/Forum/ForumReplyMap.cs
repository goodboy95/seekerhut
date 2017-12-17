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
            entity.HasKey(p => p.ForumReplyID);
            entity.Property(p => p.ForumReplyID).HasColumnName("forum_reply_id").UseMySqlIdentityColumn();
            entity.Property(p => p.FatherID).HasColumnName("father_id");
            entity.Property(p => p.CreateTime).HasColumnName("datetime");
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted");
            entity.Property(p => p.AuthorID).HasColumnName("author_id").IsRequired();
            entity.Property(p => p.Content).HasColumnName("content").IsRequired();
            entity.Property(p => p.ForumPostID).HasColumnName("forum_post_id");
        }
    }
}