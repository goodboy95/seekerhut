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
            entity.HasKey(p => p.ID);
            entity.Property(p => p.ID).HasColumnName("id").UseMySqlIdentityColumn();
            entity.Property(p => p.CreateTime).HasColumnName("create_time");
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted");
            entity.Property(p => p.Title).HasColumnName("title").IsRequired();
            entity.Property(p => p.AuthorID).HasColumnName("author_id").IsRequired();
            entity.Property(p => p.Privacy).HasColumnName("privacy").IsRequired();
            entity.Property(p => p.LikeNum).HasColumnName("like_num").IsRequired();
            entity.Property(p => p.Content).HasColumnName("content").IsRequired();
            entity.Property(p => p.LikeID).HasColumnName("like_id").IsRequired();
            entity.Property(p => p.DislikeID).HasColumnName("dislike_id").IsRequired();
            entity.Property(p => p.AwardGoldInfo).HasColumnName("award_gold_info").IsRequired();
            entity.Property(p => p.VisibleUserID).HasColumnName("visible_userid").IsRequired();
            entity.Property(p => p.Tags).HasColumnName("tags").IsRequired();
            entity.Property(p => p.Attachments).HasColumnName("attachments").IsRequired();
            entity.HasIndex(p => p.AuthorID);
        }
    }
}