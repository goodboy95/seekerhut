using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Domain.Entity;
using System;

namespace Domain.Mapping
{
    public static class CommonConfigMap
    {
        public static void MapCommonConfig(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<CommonConfigEntity>();

            entity.ToTable("common_data");
            entity.HasKey(p => p.CommonConfigID);
            entity.Property(p => p.CommonConfigID).HasColumnName("common_config_id").UseMySqlIdentityColumn();
            entity.Property(p => p.Item).HasColumnName("item").IsRequired();
            entity.Property(p => p.Value).HasColumnName("value").IsRequired();
        }
    }
}