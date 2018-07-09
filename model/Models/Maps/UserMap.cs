using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Domain.Entity;
using System;
using Newtonsoft.Json.Linq;

namespace Domain.Mapping
{
    public static class UserMap
    {
        public static void MapUser(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<UserEntity>();

            entity.ToTable("user");
            entity.HasKey(p => p.ID);
            entity.Property(p => p.ID).HasColumnName("id");
            entity.Property(p => p.CreateTime).HasColumnName("create_time");
            entity.Property(p => p.IsDeleted).HasColumnName("is_deleted");
            entity.Property(p => p.Name).HasColumnName("name").HasColumnType("varchar(30)").IsRequired();
            entity.Property(p => p.Pass).HasColumnName("pass").HasColumnType("varchar(64)").IsRequired();
            entity.Property(p => p.Salt).HasColumnName("salt").HasColumnType("varchar(64)").IsRequired();
            entity.Property(p => p.Token).HasColumnName("token").HasColumnType("varchar(64)");
            entity.Property(p => p.ExpireTime).HasColumnName("expire_time");
            entity.Property(p => p.LastLoginTime).HasColumnName("last_login_time");
            entity.Property(p => p.LastLoginIP).HasColumnName("last_login_ip").HasColumnType("varchar(40)");
            entity.Property(p => p.Admin).HasColumnName("admin");
            entity.Property(p => p.Gold).HasColumnName("gold");
            entity.Property(p => p.Exp).HasColumnName("exp");
            entity.Property(p => p.NoticeNum).HasColumnName("notice_num");
            entity.Property(p => p.LoginIP).HasColumnName("login_ip").HasColumnType("json");
            entity.HasIndex(p => p.Name).IsUnique();
            //entity.Ignore(p => p.LoginIP);
        }
    }
}
