using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using System.Collections.Generic;

namespace Domain.Mapping
{
    public static class BlogMap
    {
        public static void MapBlog(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<BlogEntity>();

            entity.ToTable("blog");
            entity.HasKey(p => p.BlogID);
            entity.Property(p => p.BlogID).HasColumnName("blog_id").UseMySqlIdentityColumn();
            entity.Property(p => p.BlogCreateTime).HasColumnName("blog_create_time");
            entity.Property(p => p.BlogIsDeleted).HasColumnName("blog_is_deleted");
            entity.Property(p => p.BlogTitle).HasColumnName("blog_title").IsRequired();
            entity.Property(p => p.BlogAuthorID).HasColumnName("blog_author_id").IsRequired();
            entity.Property(p => p.BlogPrivacy).HasColumnName("blog_privacy").IsRequired();
            entity.Property(p => p.BlogLikeNum).HasColumnName("blog_like_num").IsRequired();
            entity.Property(p => p.BlogContent).HasColumnName("blog_content").IsRequired();
            entity.Property(p => p._blogLikeID).HasColumnName("blog_like_id").IsRequired();
            entity.Property(p => p._blogDislikeID).HasColumnName("blog_dislike_id").IsRequired();
            entity.Property(p => p._blogAwardGoldInfo).HasColumnName("blog_award_gold_info").IsRequired();
            entity.Property(p => p._blogVisibleUserID).HasColumnName("blog_visible_userid").IsRequired();
            entity.Property(p => p._blogTags).HasColumnName("blog_tags").IsRequired();
            entity.Property(p => p._blogAttachments).HasColumnName("blog_attachments").IsRequired();
            entity.HasIndex(p => p.BlogAuthorID);
            entity.Ignore(p => p.BlogVisibleUserID);
            entity.Ignore(p => p.BlogTags);
            entity.Ignore(p => p.BlogLikeID);
            entity.Ignore(p => p.BlogDislikeID);
            entity.Ignore(p => p.BlogAwardGoldInfo);
            entity.Ignore(p => p.BlogAttachments);
        }
    }
}