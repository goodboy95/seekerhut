using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Dao;
using Utils;
using Domain.Entity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;

namespace web.Api.Controllers
{
    [Route("api/forum")]
    public class ForumApiController : ApiBaseController
    {
        public ForumApiController(DwDbContext dbc, ILoggerFactory logFac, IServiceProvider svp) : base(dbc, logFac, svp)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("forumList")]
        public JsonReturn GetForumList(int pageNo = 0, int pageSize = 0)
        {
            var forumList = from fl in dbc.Forum select fl;
            return JsonReturn.ReturnSuccess(new { forumList = forumList });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="forumId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("postList")]
        public JsonReturn GetPostList(int forumId, int pageNo, int pageSize)
        {
            int skipRows = (pageNo - 1) * pageSize;
            var postList = from pl in dbc.ForumPost where pl.ForumID == forumId select new {pl.ForumID, pl.AuthorID, pl.Title, pl.ViewLevel};
            if (postList.Count() > skipRows || pageNo == 1) 
            {
                postList = postList.Skip(skipRows).Take(pageSize);
                return JsonReturn.ReturnSuccess(new {postList = postList});
            }
            else
            {
                return JsonReturn.ReturnFail("页码超出范围！");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("postContent")]
        public JsonReturn GetPostContent(int postId, int pageNo, int pageSize)
        {
            int skipped = (pageNo - 1) * pageSize;
            var postContent = dbc.ForumPost.Find(postId);
            var replyList = from pl in dbc.ForumReply where pl.ForumPostID == postId select pl;
            if (replyList.Count() > skipped) 
            {
                replyList = replyList.Skip(skipped).Take(pageSize);
                return JsonReturn.ReturnSuccess(new {post = postContent, replyList = replyList });
            }
            else
            {
                return JsonReturn.ReturnFail("页码超出范围！");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="forumId"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("sendpost")]
        public JsonReturn SendPost(int forumId, string title, string content)
        {
            title = HTMLEntity.XSSConvert(title);
            var authorid = Convert.ToInt64(Request.Cookies["ID"]);
            var post = new ForumPostEntity{ForumID = forumId, Title = title, AuthorID = authorid, Content = content, ReplyID = new List<long>() };
            dbc.ForumPost.Add(post);
            dbc.SaveChanges();
            return JsonReturn.ReturnSuccess();
        }
    }
}
