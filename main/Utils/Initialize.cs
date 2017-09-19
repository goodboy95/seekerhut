using Dao;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Utils
{
    public class Initialize
    {
        public static void DbInit(DwDbContext c)
        {
            c.Database.EnsureCreated();
            //c.Database.Migrate();
            c.CommonData.Add(new CommonDataEntity{Item = "BlogPageSize", Value = "20"});
            c.CommonData.Add(new CommonDataEntity{Item = "BlogReplyPageSize", Value = "10"});
            c.CommonData.Add(new CommonDataEntity{Item = "ForumPostPageSize", Value = "20"});
            c.CommonData.Add(new CommonDataEntity{Item = "ForumReplyPageSize", Value = "20"});
            c.CommonData.Add(new CommonDataEntity{Item = "ForumSubReplyPageSize", Value = "10"});
            c.SaveChanges();
        }
    }
}