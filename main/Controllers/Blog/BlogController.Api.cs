using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Utils;
using Domain.Entity;
using Microsoft.Extensions.Logging;
using Dao;
using System.Collections.Generic;
using model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace web.Api.Controllers
{
    [Route("[controller]")]
    public class BlogApiController : ApiBaseController
    {       
        public BlogApiController(DwDbContext dbc, ILoggerFactory logFac, IServiceProvider svp) : base(dbc, logFac, svp){}

        private string dateTimeFormatter(DateTime dt)
        {
            if (dt.Date < DateTime.Today.Date) { return dt.ToString("yyyy-MM-dd"); }
            else { return dt.ToString("HH:mm"); }
        }

        [HttpGet("blog_list")]
        public JsonReturn GetBlogList(long userID, int pageNo, int pageSize)
        {
            var skipRows = (pageNo - 1) * pageSize;
            userID = userID > 0 ? userID : Convert.ToInt64(Request.Cookies["id"]);
            var blogList = dbc.Blog.Where(b => b.AuthorID == userID)
                            .Join(dbc.User, b => b.AuthorID, u => u.ID, (b, u) => new { id = b.ID, title = b.Title, create_time = b.CreateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            var blogNum = blogList.Count();
            if (blogNum > skipRows || pageNo == 1) 
            {
                if (pageNo >= 1) { blogList = blogList.Skip(skipRows); }
                if (pageSize >= 1) { blogList = blogList.Take(pageSize); }
                return JsonReturn.ReturnSuccess(new { blogNum = blogNum, blogList = blogList });
            }
            else{ return JsonReturn.ReturnFail("页码超出范围！"); }
        }

        [HttpGet("blog")]
        public JsonReturn GetBlog(long id)
        {
            var blog = dbc.Blog.Find(id);
            if (blog == null) { return JsonReturn.ReturnFail("该日志不存在！"); }
            if ((blog.Privacy & 0b10) != 0 && userID != blog.AuthorID) 
                return JsonReturn.ReturnFail("你无权访问该日志！");
            else if ((blog.Privacy & 0b01) != 0)
            {
                var visibleIds = blog.VisibleUserID;
                if (visibleIds.IndexOf(userID) == -1)
                    return JsonReturn.ReturnFail("你无权访问该日志！");
            }
            var blogInfo = JObject.Parse(JsonConvert.SerializeObject(blog));
            var authorID = blog.AuthorID;
            blogInfo["CreateTimeStr"] = dateTimeFormatter(blog.CreateTime);
            blogInfo["AuthorName"] = (from u in dbc.User where u.ID == authorID select u.Name).FirstOrDefault();
            if (blogInfo["AuthorName"] == null) { blogInfo["AuthorName"] = "幽灵用户"; }
            return JsonReturn.ReturnSuccess(blogInfo);
        }
        
        [HttpPost("blog")]
        public JsonReturn SaveBlog(long id, string title, string content, int privacy, HashSet<string> tags)
        {
            if (title == null || content == null)
            {
                return JsonReturn.ReturnFail("你有未输入的部分，无法提交！");
            }
            title = HTMLEntity.XSSConvert(title);
            var tagSet = new HashSet<string>();
            var blog = new BlogEntity { Title = title, AuthorID = userID, Privacy = privacy, VisibleUserID = new List<long>(), Tags = new HashSet<string>(), 
                                                LikeNum = 0, Content = content, Attachments = new List<FileMetaEntity>(), LikeID = new HashSet<long>(),
                                                DislikeID = new HashSet<long>(), AwardGoldInfo = new Dictionary<long, int>() };
            dbc.Blog.Add(blog);
            dbc.SaveChanges();  //提前保存一遍，以便获取博客的id
            foreach(var i in tags)
            {
                tagSet.Add(i);
                var tagUser = (from tr in dbc.BlogTagRelation where tr.UserID == userID && tr.TagName == i select tr).FirstOrDefault();
                if (tagUser == null)
                {
                    tagUser = new BlogTagRelationEntity{TagName = i, UserID = userID, BlogIDList = new List<long>()};
                }
                var tmpBlogIDList = tagUser.BlogIDList;
                tmpBlogIDList.Add(blog.ID);
                tagUser.BlogIDList = tmpBlogIDList;
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
            FileMetaEntity pic = dbc.FileMeta.Find(picId);
            if (pic == null) { return JsonReturn.ReturnFail("图片id不存在！"); }
            return JsonReturn.ReturnSuccess(new JObject(){["PicPath"] = pic.Path});
        }

        [HttpGet("tagList")]
        public JsonReturn GetTagList([FromQuery]long userId)
        {
            var tagList = from t in dbc.BlogTagRelation where t.UserID == userId select new{tagName = t.TagName};
            return JsonReturn.ReturnSuccess(tagList);
        }

        [HttpGet("blogsByTag")]
        public JsonReturn GetBlogsByTag([FromQuery]string tagName, [FromQuery]long userID)
        {
            var blogList = (from bl in dbc.BlogTagRelation where bl.TagName == tagName && bl.UserID == userID select bl.BlogIDList).FirstOrDefault();
            if (blogList == null) { blogList = new List<long>(); }
            return JsonReturn.ReturnSuccess(blogList);
        }

        [HttpGet("replyList")]
        public JsonReturn GetReply([FromQuery]long blogID, [FromQuery]int pageNo, [FromQuery]int pageSize)
        {
            if (pageNo < 1) { pageNo = 1; }
            if (pageSize < 5) { pageSize = 5; }
            var skipRows = (pageNo - 1) * pageSize;
            var replyList = from r in dbc.BlogReply where r.BlogID == blogID join u in dbc.User on r.AuthorID equals u.ID orderby r.ID
                            select new {author = u.Name, content = r.Content, sendtime = dateTimeFormatter(r.CreateTime)};
            var replyNum = replyList.Count();
            if (replyNum > skipRows || pageNo == 1)
            {
                replyList = replyList.Skip(skipRows).Take(pageSize);
                var replyListStr = JsonConvert.SerializeObject(replyList);
                var replyInfo = new JObject(){["ReplyNum"] = replyNum, ["ReplyList"] = JArray.Parse(replyListStr)};
                return JsonReturn.ReturnSuccess(replyInfo);
            }
            else
            {
                return JsonReturn.ReturnFail("页码超出范围！");
            }
        }
        
        [HttpPost("reply")]
        public JsonReturn SaveReply([FromForm]long blogAuthorID, [FromForm]long blogID, [FromForm]string content, [FromForm]long fatherID)
        {
            var bre = new BlogReplyEntity(){BlogID = blogID, AuthorID = userID, Content = content, LikeID = new HashSet<long>(), FatherID = fatherID};
            dbc.BlogReply.Add(bre);
            dbc.SaveChanges();
            wsa.WriteMsg(blogAuthorID, MessageType.BlogReply);
            return JsonReturn.ReturnSuccess();
        }

       /* [HttpPost("like")]
        public JsonReturn SendLike([FromForm]long authorID, [FromForm]string type)
        {
            
        }*/
    }
}
