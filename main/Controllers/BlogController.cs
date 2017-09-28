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
        public BlogController(DwDbContext dbc, ILoggerFactory logFac) : base(dbc, logFac)
        {
            _blogApi = new BlogApiController(dbc, logFac);
        }
        [Route("index/{id?}")]
        public IActionResult Index(string id)
        {
            var userID = Convert.ToInt64(Request.Cookies["id"]);
            int.TryParse(id, out int pageNum);
            if (pageNum < 1) { pageNum = 1; }
            var blogListReturn = _blogApi.GetBlogList(userID, pageNum, 15);
            if (blogListReturn.Code != 0) { return Redirect("/error/page404"); }
            else
            {
                var blogListInfo = (JObject)blogListReturn.Data;
                var blogListHtml = "";
                foreach (var blogLink in blogListInfo["BlogList"])
                {
                    blogListHtml += $"<a href=../blogcontent/{blogLink["BlogID"]}>{blogLink["BlogTitle"]}</a><br />\n\t";
                }
                ViewBag.BlogNum = blogListInfo["BlogNum"].ToString();
                ViewBag.PageNum = id;
                ViewBag.BlogList = blogListHtml;
                return View();
            }
        }
        [Route("writeblog")]
        public IActionResult WriteBlog() => View();
        [Route("blogcontent/{id}")]
        public IActionResult BlogContent(string id)
        {            
            var isIdNum = long.TryParse(id, out long blogId);
            var blogReturn = _blogApi.GetBlog(blogId);
            if (blogReturn.Code != 0) { return Redirect("/error/page404"); }
            else
            {
                var blogInfo = (JObject)blogReturn.Data;
                ViewBag.BlogId = id;
                ViewBag.BlogTitle = blogInfo["BlogTitle"].ToString();
                ViewBag.BlogAuthor = blogInfo["AuthorName"].ToString();
                ViewBag.BlogContent = blogInfo["BlogContent"].ToString();
                ViewBag.CreateTime = blogInfo["CreateTimeStr"].ToString();
                return View();
            }
        }
    }
}
