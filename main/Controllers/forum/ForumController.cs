using Microsoft.AspNetCore.Mvc;
using Dao;
using Microsoft.Extensions.Logging;

namespace web.Controllers
{
    public class ForumController : ViewBaseController
    {
        public ForumController(DwDbContext dbc, ILoggerFactory logFac) : base(dbc, logFac)
        {
        }
        public IActionResult Index() => View();
        public IActionResult PostList() => View();
        public IActionResult PostContent() => View();

    }
}
