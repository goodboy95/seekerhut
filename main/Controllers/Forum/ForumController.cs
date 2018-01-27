using Microsoft.AspNetCore.Mvc;
using Dao;
using Microsoft.Extensions.Logging;
using System;

namespace web.Controllers
{
    public class ForumController : ViewBaseController
    {
        public ForumController(DwDbContext dbc, ILoggerFactory logFac, IServiceProvider svp) : base(dbc, logFac, svp)
        {
        }
        public IActionResult Index() => View();
        public IActionResult PostList() => View();
        public IActionResult PostContent() => View();

    }
}
