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
            entity.HasKey(p => p.QuizID);
            entity.Property(p => p.QuizID).HasColumnName("quiz_id");
            entity.Property(p => p.QuizCreateTime).HasColumnName("quiz_create_time");
            entity.Property(p => p.QuizCreator).HasColumnName("quiz_creator");
            entity.Property(p => p.QuizIsDeleted).HasColumnName("quiz_is_deleted");
            entity.Property(p => p.QuizName).HasColumnName("quiz_name").HasColumnType("varchar(80)").IsRequired();
            entity.Property(p => p.QuizIntro).HasColumnName("quiz_intro").HasColumnType("varchar(255)");
            entity.Property(p => p.QuizPicPath).HasColumnName("quiz_picpath");
            entity.Property(p => p.QuizBody).HasColumnName("quiz_body").IsRequired();
            entity.Property(p => p._quizLikes).HasColumnName("quiz_likes").IsRequired();
            entity.Ignore(p => p.QuizLikes);
        }
    }
}