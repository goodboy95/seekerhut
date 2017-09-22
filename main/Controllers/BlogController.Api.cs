using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Utils;
using Domain.Entity;
using Microsoft.Extensions.Logging;
using Dao;
using System.Collections.Generic;
using model;

namespace web.Api.Controllers
{
    [Route("api/[controller]")]
    public class BlogController : ApiBaseController
    {
        //StackRedisHelper redis;
        public BlogController(DwDbContext dbc, ILoggerFactory logFac) : base(dbc, logFac)
        {
            //redis = StackRedisHelper.Instance;
        }
        [HttpGet("blogList")]
        public JsonReturn GetBlogList([FromQuery]long authorID, [FromQuery]int pageNo, [FromQuery]int pageSize)
        {
            var skipRows = (pageNo - 1) * pageSize;
            var blogList = from blog in dbc.Blog where blog.AuthorID == authorID select new {blog.Title, blog.ID, blog.CreateTime};
            var blogNum = blogList.Count();
            if (blogNum > skipRows || pageNo == 1) 
            {
                blogList = blogList.Skip(skipRows).Take(pageSize);
            return JsonReturn.ReturnSuccess(new { blogNum = blogNum, blogList = blogList });
            }
            else
            {
                return JsonReturn.ReturnFail("页码超出范围！");
            }
            
        }
        [HttpGet("blog")]
        public JsonReturn GetBlog([FromQuery]int id)
        {
            var blog = dbc.Blog.Find(Convert.ToInt64(id));
            var authorID = blog.AuthorID;
            var ct = blog.CreateTime;
            string createTimeStr = $"{ct.Year}年{ct.Month}月{ct.Day}日  {ct.Hour}时{ct.Minute}分";
            var authorName = (from u in dbc.User where u.ID == authorID select u.Name).FirstOrDefault();
            if (authorName == null) { authorName = "幽灵用户"; }
            if (blog == null) { return JsonReturn.ReturnFail("该日志不存在！"); }
            if ((blog.Privacy & 0b10) != 0 && Convert.ToInt64(Request.Cookies["id"]) != blog.AuthorID) 
                return JsonReturn.ReturnFail("你无权访问该日志！");
            else if ((blog.Privacy & 0b01) != 0)
            {
                var userid = Convert.ToInt64(Request.Cookies["id"]);
                var visibleIds = blog.VisibleUserID;
                if (visibleIds.IndexOf(userid) == -1)
                    return JsonReturn.ReturnFail("你无权访问该日志！");
            }
            return JsonReturn.ReturnSuccess(new {blog = blog, authorName = authorName, createTime = createTimeStr});
        }
        
        [HttpPost("blog")]
        public JsonReturn SaveBlog([FromForm]long id, [FromForm]string title, [FromForm]string content, [FromForm]int privacy, [FromForm]HashSet<string> tags)
        {
            if (title == null || content == null)
            {
                return JsonReturn.ReturnFail("你有未输入的部分，无法提交！");
            }
            title = HTMLEntity.XSSConvert(title);
            long authorID = Convert.ToInt64(Request.Cookies["id"]);
            var tagSet = new HashSet<string>();
            BlogEntity blog = new BlogEntity { Title = title, AuthorID = authorID, Privacy = privacy, VisibleUserID = new List<long>(), Tags = new HashSet<string>(), 
                                                LikeNum = 0, Content = content, Attachments = new List<AttachObj>(), LikeID = new HashSet<long>(),
                                                DislikeID = new HashSet<long>(), AwardGoldInfo = new Dictionary<long, int>() };
            dbc.Blog.Add(blog);
            dbc.SaveChanges();  //提前保存一遍，以便获取博客的id
            foreach(var i in tags)
            {
                tagSet.Add(i);
                var tagUser = (from tr in dbc.BlogTagRelation where tr.UserID == authorID && tr.TagName == i select tr).FirstOrDefault();
                if (tagUser == null)
                {
                    tagUser = new BlogTagRelationEntity{TagName = i, UserID = authorID, BlogID = new List<long>()};
                }
                var tmpBlogID = tagUser.BlogID;
                tmpBlogID.Add(blog.ID);
                tagUser.BlogID = tmpBlogID;
                if (tagUser.ID == 0) { dbc.BlogTagRelation.Add(tagUser); }
                else { dbc.BlogTagRelation.Update(tagUser); };
            }
            blog.Tags = tagSet;
            dbc.SaveChanges();
            return JsonReturn.ReturnSuccess();
        }
        [HttpGet("Pic")]
        public JsonReturn Pic([FromQuery]int picId)
        {
            PictureEntity pic = dbc.Picture.Find(picId);
            if (pic == null) { return JsonReturn.ReturnFail("图片id不存在！"); }
            return JsonReturn.ReturnSuccess(new {picPath = pic.PicPath});
        }
        [HttpGet("tagList")]
        public JsonReturn GetTagList([FromQuery]long userId)
        {
            var tagList = from t in dbc.BlogTagRelation where t.UserID == userId select new{tagName = t.TagName};
            return JsonReturn.ReturnSuccess(new {tagList = tagList});
        }
        [HttpGet("blogsByTag")]
        public JsonReturn GetBlogsByTag([FromQuery]string tagName, [FromQuery]long userID)
        {
            var blogList = (from bl in dbc.BlogTagRelation where bl.TagName == tagName && bl.UserID == userID select bl.BlogID).FirstOrDefault();
            if (blogList == null) { blogList = new List<long>(); }
            return JsonReturn.ReturnSuccess(new {blogList = blogList});
        }
        [HttpGet("replyList")]
        public JsonReturn GetReply([FromQuery]long blogID, [FromQuery]int pageNo, [FromQuery]int pageSize)
        {
            if (pageNo <= 0) { pageNo = 1; }
            if (pageSize <= 2) { pageSize = 5; }
            var skipRows = (pageNo - 1) * pageSize;
            var replyList = from r in dbc.BlogReply where r.BlogID == blogID select r;
            var replyNum = replyList.Count();
            var pageNum = replyNum / pageSize + (replyNum % pageSize > 0 ? 1 : 0);
            if (replyNum > skipRows || pageNo == 1)
            {
                replyList = replyList.Skip(skipRows).Take(pageSize);
                return JsonReturn.ReturnSuccess(new {replyList = replyList, pageNum = pageNum});
            }
            else
            {
                return JsonReturn.ReturnFail("页码超出范围！");
            }
        }
        [HttpPost("reply")]
        public JsonReturn SaveReply([FromForm]long authorID, [FromForm]long blogID, [FromForm]string content, [FromForm]long reReplyID)
        {
            var userID = Convert.ToInt64(Request.Cookies["id"]);
            var bre = new BlogReplyEntity(){BlogID = blogID, AuthorID = userID, Content = content, LikeID = new HashSet<long>(), ReReplyID = reReplyID};
            dbc.BlogReply.Add(bre);
            dbc.SaveChanges();
            //var notifyMsg = new { msgType = MessageType.blogReply.ToInt(), sender = userID }.ToString();
            wsa.WriteMsg(authorID, "blogNotice");
            return JsonReturn.ReturnSuccess();
        }
    }
}
