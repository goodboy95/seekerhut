using System.Linq;
using Dao;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Utils
{
    public class Initialize
    {
        public static void DbInit(DwDbContext c)
        {
            //c.Database.Migrate();
            c.Database.EnsureCreated();
            if (!c.CommonConfig.Any())
            {
                c.CommonConfig.Add(new CommonConfigEntity{Item = "BlogPageSize", Value = "20"});
                c.CommonConfig.Add(new CommonConfigEntity{Item = "BlogReplyPageSize", Value = "10"});
                c.CommonConfig.Add(new CommonConfigEntity{Item = "ForumPostPageSize", Value = "20"});
                c.CommonConfig.Add(new CommonConfigEntity{Item = "ForumReplyPageSize", Value = "20"});
                c.CommonConfig.Add(new CommonConfigEntity{Item = "ForumSubReplyPageSize", Value = "10"});
                c.SaveChanges();
            }
        }
    }
}