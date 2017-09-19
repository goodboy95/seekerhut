using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Domain.Entity;
using System;

namespace Domain.Mapping
{
    public static class PictureMap
    {
        public static void MapPicture(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<PictureEntity>();

            entity.ToTable("picture");
            entity.HasKey(p => p.ID);
            entity.Property(p => p.ID).HasColumnName("id").UseMySqlIdentityColumn();
            entity.Property(p => p.CreateTime).HasColumnName("datetime");
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted");
            entity.Property(p => p.PicName).HasColumnName("pic_name").HasColumnType("varchar(50)").IsRequired();
            entity.Property(p => p.PicSha256).HasColumnName("pic_sha256").HasColumnType("varchar(64)").IsRequired();
            entity.Property(p => p.PicSize).HasColumnName("pic_size").IsRequired();
            entity.Property(p => p.PicPath).HasColumnName("pic_path").HasColumnType("varchar(100)").IsRequired();
        }
    }
}