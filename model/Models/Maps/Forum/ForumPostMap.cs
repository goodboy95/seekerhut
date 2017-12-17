using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Domain.Entity;
using System.Collections.Generic;
using System;

namespace Domain.Mapping
{
    public static class ForumPostMap
    {
        public static void MapForumPost(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<ForumPostEntity>();

            entity.ToTable("forum_post");
            entity.HasKey(p => p.ForumPostID);
            entity.Property(p => p.ForumPostID).HasColumnName("forum_post_id").UseMySqlIdentityColumn();
            entity.Property(p => p.CreateTime).HasColumnName("datetime");
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted");
            entity.Property(p => p.ForumID).HasColumnName("forum_id").IsRequired();
            entity.Property(p => p.Title).HasColumnName("title").IsRequired();
            entity.Property(p => p.AuthorID).HasColumnName("author_id").IsRequired();
            entity.Property(p => p.Content).HasColumnName("content").IsRequired();
            entity.Property(p => p.ReplyLevel).HasColumnName("reply_level");
            entity.Property(p => p.ViewLevel).HasColumnName("view_level");
            entity.Property(p => p.Status).HasColumnName("status");
            entity.Property(p => p._replyID).HasColumnName("reply_id").IsRequired();
            entity.Ignore(p => p.ReplyID);
        }
    }
}