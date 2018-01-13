using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Domain.Entity;
using System;
using Newtonsoft.Json.Linq;

namespace Domain.Mapping
{
    public static class QuizMap
    {
        public static void MapQuiz(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<QuizEntity>();
            entity.ToTable("quiz");
            entity.HasKey(p => p.ID);
            entity.Property(p => p.ID).HasColumnName("id");
            entity.Property(p => p.CreateTime).HasColumnName("create_time");
            entity.Property(p => p.CreatorID).HasColumnName("creator");
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted");
            entity.Property(p => p.Name).HasColumnName("name").HasColumnType("varchar(80)").IsRequired();
            entity.Property(p => p.Intro).HasColumnName("intro").HasColumnType("varchar(255)");
            entity.Property(p => p.PicPath).HasColumnName("picpath");
            entity.Property(p => p.Body).HasColumnName("body").IsRequired();
            entity.Property(p => p._likes).HasColumnName("likes").IsRequired();
            entity.Ignore(p => p.Likes);
        }
    }
}