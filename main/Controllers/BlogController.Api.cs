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
            var blogList = from blog in dbc.Blog where blog.BlogAuthorID == authorID select new {blog.BlogTitle, blog.BlogID, blog.BlogCreateTime};
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
            var authorID = blog.BlogAuthorID;
            var ct = blog.BlogCreateTime;
            string createTimeStr = $"{ct.Year}年{ct.Month}月{ct.Day}日  {ct.Hour}时{ct.Minute}分";
            var authorName = (from u in dbc.User where u.UserID == authorID select u.Name).FirstOrDefault();
            if (authorName == null) { authorName = "幽灵用户"; }
            if (blog == null) { return JsonReturn.ReturnFail("该日志不存在！"); }
            if ((blog.BlogPrivacy & 0b10) != 0 && Convert.ToInt64(Request.Cookies["id"]) != blog.BlogAuthorID) 
                return JsonReturn.ReturnFail("你无权访问该日志！");
            else if ((blog.BlogPrivacy & 0b01) != 0)
            {
                var userid = Convert.ToInt64(Request.Cookies["id"]);
                var visibleIds = blog.BlogVisibleUserID;
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
            BlogEntity blog = new BlogEntity { BlogTitle = title, BlogAuthorID = authorID, BlogPrivacy = privacy, BlogVisibleUserID = new List<long>(), BlogTags = new HashSet<string>(), 
                                                BlogLikeNum = 0, BlogContent = content, BlogAttachments = new List<FileMetaEntity>(), BlogLikeID = new HashSet<long>(),
                                                BlogDislikeID = new HashSet<long>(), BlogAwardGoldInfo = new Dictionary<long, int>() };
            dbc.Blog.Add(blog);
            dbc.SaveChanges();  //提前保存一遍，以便获取博客的id
            foreach(var i in tags)
            {
                tagSet.Add(i);
                var tagUser = (from tr in dbc.BlogTagRelation where tr.BtrUserID == authorID && tr.BtrTagName == i select tr).FirstOrDefault();
                if (tagUser == null)
                {
                    tagUser = new BlogTagRelationEntity{BtrTagName = i, BtrUserID = authorID, BlogIDList = new List<long>()};
                }
                var tmpBlogIDList = tagUser.BlogIDList;
                tmpBlogIDList.Add(blog.BlogID);
                tagUser.BlogIDList = tmpBlogIDList;
                if (tagUser.BtrID == 0) { dbc.BlogTagRelation.Add(tagUser); }
                else { dbc.BlogTagRelation.Update(tagUser); };
            }
            blog.BlogTags = tagSet;
            dbc.SaveChanges();
            return JsonReturn.ReturnSuccess();
        }
        [HttpGet("Pic")]
        public JsonReturn Pic([FromQuery]int picId)
        {
            FileMetaEntity pic = dbc.FileMeta.Find(picId);
            if (pic == null) { return JsonReturn.ReturnFail("图片id不存在！"); }
            return JsonReturn.ReturnSuccess(new {picPath = pic.FileMetaPath});
        }
        [HttpGet("tagList")]
        public JsonReturn GetTagList([FromQuery]long userId)
        {
            var tagList = from t in dbc.BlogTagRelation where t.BtrUserID == userId select new{tagName = t.BtrTagName};
            return JsonReturn.ReturnSuccess(new {tagList = tagList});
        }
        [HttpGet("blogsByTag")]
        public JsonReturn GetBlogsByTag([FromQuery]string tagName, [FromQuery]long userID)
        {
            var blogList = (from bl in dbc.BlogTagRelation where bl.BtrTagName == tagName && bl.BtrUserID == userID select bl.BlogIDList).FirstOrDefault();
            if (blogList == null) { blogList = new List<long>(); }
            return JsonReturn.ReturnSuccess(new {blogList = blogList});
        }
        [HttpGet("replyList")]
        public JsonReturn GetReply([FromQuery]long blogID, [FromQuery]int pageNo, [FromQuery]int pageSize)
        {
            if (pageNo <= 0) { pageNo = 1; }
            if (pageSize <= 2) { pageSize = 5; }
            var skipRows = (pageNo - 1) * pageSize;
            var replyList = from r in dbc.BlogReply where r.BlogID == blogID
                            join u in dbc.User on r.BlogReplyAuthorID equals u.UserID select r;
            var replyNum = replyList.Count();
            if (replyNum > skipRows || pageNo == 1)
            {
                replyList = replyList.Skip(skipRows).Take(pageSize);
                return JsonReturn.ReturnSuccess(new {replyList = replyList, replyNum = replyNum});
            }
            else
            {
                return JsonReturn.ReturnFail("页码超出范围！");
            }
        }
        [HttpPost("reply")]
        public JsonReturn SaveReply([FromForm]long authorID, [FromForm]long blogID, [FromForm]string content, [FromForm]long fatherID)
        {
            var userID = Convert.ToInt64(Request.Cookies["id"]);
            var bre = new BlogReplyEntity(){BlogID = blogID, BlogReplyAuthorID = userID, BlogReplyContent = content, BlogReplyLikeID = new HashSet<long>(), BlogReplyFatherID = fatherID};
            dbc.BlogReply.Add(bre);
            dbc.SaveChanges();
            wsa.WriteMsg(authorID, MessageType.BlogReply);
            return JsonReturn.ReturnSuccess();
        }
    }
}
