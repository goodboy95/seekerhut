using System;
using System.Linq;
using Dao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Utils;

namespace web.Api.Controllers
{
    [Route("api/admin")]
    public class AdminApiController : ApiBaseController
    {
        public AdminApiController(DwDbContext dbc, ILoggerFactory logFac, IServiceProvider svp) : base(dbc, logFac, svp){}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("my_blog_list")]
        public JsonReturn MyBlogList(int pageNo, int pageSize)
        {
            var skipRows = (pageNo - 1) * pageSize;
            var userID = Convert.ToInt64(Request.Cookies["id"]);
            var blogList = dbc.Blog.Where(b => b.AuthorID == userID)
                            .Select(b => new { id = b.ID, title = b.Title, create_time = b.CreateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            var blogNum = blogList.Count();
            if (blogNum > skipRows || pageNo == 1) 
            {
                if (pageNo >= 1) { blogList = blogList.Skip(skipRows); }
                if (pageSize >= 1) { blogList = blogList.Take(pageSize); }
                return JsonReturn.ReturnSuccess(new { blogNum = blogNum, blogList = blogList });
            }
            else{ return JsonReturn.ReturnFail("页码超出范围！"); }
        } 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("my_blog_reply_list")]
        public JsonReturn MyBlogReplyList(int pageNo, int pageSize)
        {
            var skipRows = (pageNo - 1) * pageSize;
            var userID = Convert.ToInt64(Request.Cookies["id"]);
            var replyList = dbc.BlogReply.Where(b => b.AuthorID == userID)
                            .Join(dbc.Blog, r => r.BlogID, b => b.ID, (r, b) => new { 
                                id = r.ID,
                                blog_id = b.ID,
                                blog_title = b.Title, 
                                create_time = r.CreateTime.ToString("yyyy-MM-dd hh:mm:ss"), 
                                content = r.Content
                            });
            var blogNum = replyList.Count();
            if (blogNum > skipRows || pageNo == 1) 
            {
                if (pageNo >= 1) { replyList = replyList.Skip(skipRows); }
                if (pageSize >= 1) { replyList = replyList.Take(pageSize); }
                return JsonReturn.ReturnSuccess(new { blogNum = blogNum, replyList = replyList });
            }
            else{ return JsonReturn.ReturnFail("页码超出范围！"); }
        }
    }
}