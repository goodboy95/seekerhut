using Microsoft.EntityFrameworkCore;
using Domain.Entity;
using Domain.Mapping;


namespace Dao
{
    public class DwDbContext : DbContext
    {
        public DwDbContext(DbContextOptions<DwDbContext> options) : base(options)
        {
        }
        public DbSet<UserEntity> User { get; set; }
        public DbSet<CommonConfigEntity> CommonConfig { get; set; }
        public DbSet<FileMetaEntity> FileMeta { get; set; }
        public DbSet<BlogEntity> Blog { get; set; }
        public DbSet<BlogReplyEntity> BlogReply { get; set; }
        public DbSet<BlogTagRelationEntity> BlogTagRelation { get; set; }
        public DbSet<BlogTagsEntity> BlogTags { get; set; }
        public DbSet<QuizEntity> Quiz { get; set; }
        public DbSet<AnswerEntity> Answer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.MapUser();
            modelBuilder.MapFileMeta();
            modelBuilder.MapCommonConfig();
            modelBuilder.MapBlog();
            modelBuilder.MapBlogReply();
            modelBuilder.MapBlogTagRelation();
            modelBuilder.MapBlogTags();
            modelBuilder.MapQuiz();
            modelBuilder.MapAnswer();
            base.OnModelCreating(modelBuilder);
        }
    }
}