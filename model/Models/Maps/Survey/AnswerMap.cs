using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Domain.Mapping
{
    public static class AnswerMap
    {
        public static void MapAnswer(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<AnswerEntity>();
            entity.ToTable("answer");
            entity.HasKey(p => p.AnswerID);
            entity.Property(p => p.AnswerID).HasColumnName("answer_id");
            entity.Property(p => p.AnswerCreateTime).HasColumnName("answer_create_time");
            entity.Property(p => p.AnswerIsDeleted).HasColumnName("answer_is_deleted");
            entity.Property(p => p.AnswerIP).HasColumnName("answer_ip").HasColumnType("varchar(100)").IsRequired();
            entity.Property(p => p.QuizID).HasColumnName("quiz_id").IsRequired();
            entity.Property(p => p.AnswerCreator).HasColumnName("answer_creator");
            entity.Property(p => p.AnswerBody).HasColumnName("answer_body").IsRequired();
            //entity.Ignore(p => p.AnswerBody);
        }
    }
}