using Microsoft.AspNetCore.Mvc;
using Dao;
using Microsoft.Extensions.Logging;
using System;

namespace web.Controllers
{
    [Route("forum")]
    public class ForumController : ViewBaseController
    {
        public ForumController(DwDbContext dbc, ILoggerFactory logFac, IServiceProvider svp) : base(dbc, logFac, svp)
        {
        }
        [HttpGet("index")]
        public IActionResult Index() => View();
        [HttpGet("postlist")]
        public IActionResult PostList() => View();
        [HttpGet("postcontent")]
        public IActionResult PostContent() => View();

    }
}
