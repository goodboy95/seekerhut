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
            entity.HasKey(p => p.FileMetaID);
            entity.Property(p => p.FileMetaID).HasColumnName("file_meta_id").UseMySqlIdentityColumn();
            entity.Property(p => p.FileMetaCreateTime).HasColumnName("file_meta_create_time");
            entity.Property(p => p.FileMetaIsDeleted).HasColumnName("file_meta_is_deleted");
            entity.Property(p => p.FileMetaName).HasColumnName("file_meta_name").HasColumnType("varchar(50)").IsRequired();
            entity.Property(p => p.FileMetaSha256).HasColumnName("file_meta_sha256").HasColumnType("varchar(64)").IsRequired();
            entity.Property(p => p.FileMetaSize).HasColumnName("file_meta_size").IsRequired();
            entity.Property(p => p.FileMetaPath).HasColumnName("file_meta_path").HasColumnType("varchar(100)").IsRequired();
        }
    }
}