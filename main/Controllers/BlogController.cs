using Microsoft.AspNetCore.Mvc;
using Dao;
using Microsoft.Extensions.Logging;
using web.Api.Controllers;
using Newtonsoft.Json.Linq;
using System;

namespace web.Controllers
{
    [Route("[controller]")]
    public class BlogController : ViewBaseController
    {
        private readonly BlogApiController _blogApi;
        public BlogController(DwDbContext dbc, ILoggerFactory logFac, IServiceProvider svp) : base(dbc, logFac, svp)
        {
            _blogApi = new BlogApiController(dbc, logFac, svp);
        }

        [Route("index/{pageNumStr?}")]
        public IActionResult Index(string pageNumStr)
        {
            var userID = Convert.ToInt64(Request.Cookies["id"]);
            int.TryParse(pageNumStr, out int pageNum);
            if (pageNum < 1) { pageNum = 1; }
            var blogListReturn = _blogApi.GetBlogList(userID, pageNum, 15);
            if (blogListReturn.Code != 0) { return Redirect("/error/page404"); }
            else
            {
                var blogListInfo = (JObject)blogListReturn.Data;
                var blogListHtml = "";
                foreach (var blogLink in blogListInfo["BlogList"])
                {
                    blogListHtml += $"<a href=../content/{blogLink["BlogID"]}>{blogLink["BlogTitle"]}</a><br />\n\t";
                }
                ViewBag.BlogNum = blogListInfo["BlogNum"].ToString();
                ViewBag.PageNum = pageNum;
                ViewBag.BlogList = blogListHtml;
                return View();
            }
        }

        [Route("writeblog")]
        public IActionResult WriteBlog() => View();

        [Route("content/{id}")]
        public IActionResult BlogContent(string id)
        {            
            var isIdNum = long.TryParse(id, out long blogId);
            var blogReturn = _blogApi.GetBlog(blogId);
            if (blogReturn.Code != 0) { return Redirect("/error/page404"); }
            else
            {
                var blogInfo = (JObject)blogReturn.Data;
                ViewBag.BlogID = id;
                ViewBag.AuthorID = blogInfo["BlogAuthorID"].ToString();
                ViewBag.BlogTitle = blogInfo["BlogTitle"].ToString();
                ViewBag.BlogAuthor = blogInfo["AuthorName"].ToString();
                ViewBag.BlogContent = blogInfo["BlogContent"].ToString();
                ViewBag.CreateTime = blogInfo["CreateTimeStr"].ToString();
                return View();
            }
        }
    }
}
