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

        [HttpGet("blogList")]
        public JsonReturn GetBlogList([FromQuery]long authorID, [FromQuery]int pageNo, [FromQuery]int pageSize)
        {
            if (pageNo < 1) { pageNo = 1; }
            if (pageSize < 5) { pageSize = 5; }
            var skipRows = (pageNo - 1) * pageSize;
            var blogList = from blog in dbc.Blog where blog.BlogAuthorID == authorID 
            orderby blog.BlogID select new {blog.BlogTitle, blog.BlogID, blog.BlogCreateTime};
            var blogNum = blogList.Count();
            if (blogNum > skipRows || pageNo == 1) 
            {
                blogList = blogList.Skip(skipRows).Take(pageSize);
                var blogListStr = JsonConvert.SerializeObject(blogList);
                var blogListInfo = new JObject(){["BlogNum"] = blogNum, ["BlogList"] = JArray.Parse(blogListStr)};
                return JsonReturn.ReturnSuccess(blogListInfo);
            }
            else
            {
                return JsonReturn.ReturnFail("页码超出范围！");
            }
        }

        [HttpGet("blog")]
        public JsonReturn GetBlog([FromQuery]long id)
        {
            var blog = dbc.Blog.Find(id);
            if (blog == null) { return JsonReturn.ReturnFail("该日志不存在！"); }
            if ((blog.BlogPrivacy & 0b10) != 0 && userID != blog.BlogAuthorID) 
                return JsonReturn.ReturnFail("你无权访问该日志！");
            else if ((blog.BlogPrivacy & 0b01) != 0)
            {
                var visibleIds = blog.BlogVisibleUserID;
                if (visibleIds.IndexOf(userID) == -1)
                    return JsonReturn.ReturnFail("你无权访问该日志！");
            }
            var blogInfo = JObject.Parse(JsonConvert.SerializeObject(blog));
            var authorID = blog.BlogAuthorID;
            blogInfo["CreateTimeStr"] = dateTimeFormatter(blog.BlogCreateTime);
            blogInfo["AuthorName"] = (from u in dbc.User where u.UserID == authorID select u.Name).FirstOrDefault();
            if (blogInfo["AuthorName"] == null) { blogInfo["AuthorName"] = "幽灵用户"; }
            return JsonReturn.ReturnSuccess(blogInfo);
        }
        
        [HttpPost("blog")]
        public JsonReturn SaveBlog([FromForm]long id, [FromForm]string title, [FromForm]string content, [FromForm]int privacy, [FromForm]HashSet<string> tags)
        {
            if (title == null || content == null)
            {
                return JsonReturn.ReturnFail("你有未输入的部分，无法提交！");
            }
            title = HTMLEntity.XSSConvert(title);
            var tagSet = new HashSet<string>();
            BlogEntity blog = new BlogEntity { BlogTitle = title, BlogAuthorID = userID, BlogPrivacy = privacy, BlogVisibleUserID = new List<long>(), BlogTags = new HashSet<string>(), 
                                                BlogLikeNum = 0, BlogContent = content, BlogAttachments = new List<FileMetaEntity>(), BlogLikeID = new HashSet<long>(),
                                                BlogDislikeID = new HashSet<long>(), BlogAwardGoldInfo = new Dictionary<long, int>() };
            dbc.Blog.Add(blog);
            dbc.SaveChanges();  //提前保存一遍，以便获取博客的id
            foreach(var i in tags)
            {
                tagSet.Add(i);
                var tagUser = (from tr in dbc.BlogTagRelation where tr.BtrUserID == userID && tr.BtrTagName == i select tr).FirstOrDefault();
                if (tagUser == null)
                {
                    tagUser = new BlogTagRelationEntity{BtrTagName = i, BtrUserID = userID, BlogIDList = new List<long>()};
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
            return JsonReturn.ReturnSuccess(new JObject(){["PicPath"] = pic.FileMetaPath});
        }

        [HttpGet("tagList")]
        public JsonReturn GetTagList([FromQuery]long userId)
        {
            var tagList = from t in dbc.BlogTagRelation where t.BtrUserID == userId select new{tagName = t.BtrTagName};
            return JsonReturn.ReturnSuccess(tagList);
        }

        [HttpGet("blogsByTag")]
        public JsonReturn GetBlogsByTag([FromQuery]string tagName, [FromQuery]long userID)
        {
            var blogList = (from bl in dbc.BlogTagRelation where bl.BtrTagName == tagName && bl.BtrUserID == userID select bl.BlogIDList).FirstOrDefault();
            if (blogList == null) { blogList = new List<long>(); }
            return JsonReturn.ReturnSuccess(blogList);
        }

        [HttpGet("replyList")]
        public JsonReturn GetReply([FromQuery]long blogID, [FromQuery]int pageNo, [FromQuery]int pageSize)
        {
            if (pageNo < 1) { pageNo = 1; }
            if (pageSize < 5) { pageSize = 5; }
            var skipRows = (pageNo - 1) * pageSize;
            var replyList = from r in dbc.BlogReply where r.BlogID == blogID join u in dbc.User on r.BlogReplyAuthorID equals u.UserID orderby r.BlogReplyID
                            select new {author = u.Name, content = r.BlogReplyContent, sendtime = dateTimeFormatter(r.BlogReplyCreateTime)};
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
            var bre = new BlogReplyEntity(){BlogID = blogID, BlogReplyAuthorID = userID, BlogReplyContent = content, BlogReplyLikeID = new HashSet<long>(), BlogReplyFatherID = fatherID};
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
