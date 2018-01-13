using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Domain.Entity;
using System;

namespace Domain.Mapping
{
    public static class FileMetaMap
    {
        public static void MapFileMeta(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<FileMetaEntity>();

            entity.ToTable("file_meta");
            entity.HasKey(p => p.ID);
            entity.Property(p => p.ID).HasColumnName("id").UseMySqlIdentityColumn();
            entity.Property(p => p.CreateTime).HasColumnName("create_time");
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted");
            entity.Property(p => p.Name).HasColumnName("name").HasColumnType("varchar(50)").IsRequired();
            entity.Property(p => p.Sha256).HasColumnName("sha256").HasColumnType("varchar(64)").IsRequired();
            entity.Property(p => p.Size).HasColumnName("size").IsRequired();
            entity.Property(p => p.Path).HasColumnName("path").HasColumnType("varchar(100)").IsRequired();
        }
    }
}