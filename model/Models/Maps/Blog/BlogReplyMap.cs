using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Domain.Entity;
using System.Collections.Generic;
using System;

namespace Domain.Mapping
{
    public static class BlogReplyMap
    {
        public static void MapBlogReply(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<BlogReplyEntity>();

            entity.ToTable("blog_reply");
            entity.HasKey(p => p.ID);
            entity.Property(p => p.ID).HasColumnName("id").UseMySqlIdentityColumn();
            entity.Property(p => p.CreateTime).HasColumnName("datetime");
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted");
            entity.Property(p => p.BlogID).HasColumnName("blog_id");
            entity.Property(p => p.AuthorID).HasColumnName("author_id").IsRequired();
            entity.Property(p => p.Content).HasColumnName("content").IsRequired();
            entity.Property(p => p.ReReplyID).HasColumnName("re_reply_id").IsRequired();
            entity.Property(p => p._likeID).HasColumnName("like_id").IsRequired();
            entity.HasIndex(p => p.BlogID);
            entity.HasIndex(p => p.AuthorID);
            entity.Ignore(p => p.LikeID);
        }
    }
}