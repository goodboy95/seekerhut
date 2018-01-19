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
        public IActionResult Index(string pageNumStr) => View();

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
                ViewBag.AuthorID = blogInfo["AuthorID"].ToString();
                ViewBag.BlogTitle = blogInfo["Title"].ToString();
                ViewBag.BlogAuthor = blogInfo["AuthorName"].ToString();
                ViewBag.BlogContent = blogInfo["Content"].ToString();
                ViewBag.CreateTime = blogInfo["CreateTimeStr"].ToString();
                return View();
            }
        }
    }
}
