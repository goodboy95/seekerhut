using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Domain.Entity;
using System;

namespace Domain.Mapping
{
    public static class CommonDataMap
    {
        public static void MapCommonData(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<CommonDataEntity>();

            entity.ToTable("common_data");
            entity.HasKey(p => p.ID);
            entity.Property(p => p.ID).HasColumnName("id").UseMySqlIdentityColumn();
            entity.Property(p => p.CreateTime).HasColumnName("datetime").IsRequired();
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted").IsRequired();
            entity.Property(p => p.Item).HasColumnName("item").IsRequired();
            entity.Property(p => p.Value).HasColumnName("value").IsRequired();
        }
    }
}