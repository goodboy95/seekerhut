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
            entity.HasKey(p => p.ID);
            entity.Property(p => p.ID).HasColumnName("id");
            entity.Property(p => p.CreateTime).HasColumnName("create_time");
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted");
            entity.Property(p => p.SourceIP).HasColumnName("source_ip").HasColumnType("varchar(100)").IsRequired();
            entity.Property(p => p.QuizID).HasColumnName("quiz_id").IsRequired();
            entity.Property(p => p.CreatorID).HasColumnName("creator");
            entity.Property(p => p.Body).HasColumnName("body").IsRequired();
            //entity.Ignore(p => p.AnswerBody);
        }
    }
}