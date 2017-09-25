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
            entity.HasKey(p => p.BlogReplyID);
            entity.Property(p => p.BlogReplyID).HasColumnName("blog_reply_id").UseMySqlIdentityColumn();
            entity.Property(p => p.BlogReplyCreateTime).HasColumnName("blog_reply_create_time");
            entity.Property(p => p.BlogReplyIsDeleted).HasColumnName("blog_reply_is_deleted");
            entity.Property(p => p.BlogID).HasColumnName("blog_id");
            entity.Property(p => p.BlogReplyAuthorID).HasColumnName("blog_reply_author_id").IsRequired();
            entity.Property(p => p.BlogReplyContent).HasColumnName("blog_reply_content").IsRequired();
            entity.Property(p => p.BlogReplyFatherID).HasColumnName("blog_reply_father_id").IsRequired();
            entity.Property(p => p._blogReplyLikeID).HasColumnName("blog_reply_like_id").IsRequired();
            entity.HasIndex(p => p.BlogID);
            entity.HasIndex(p => p.BlogReplyAuthorID);
            entity.Ignore(p => p.BlogReplyLikeID);
        }
    }
}