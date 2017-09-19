using Microsoft.AspNetCore.Mvc;
using Dao;
using Microsoft.Extensions.Logging;

namespace web.Controllers
{
    public class BlogController : ViewBaseController
    {
        public BlogController(DwDbContext dbc, ILoggerFactory logFac) : base(dbc, logFac)
        {
        }
        public IActionResult Index() => View();
        public IActionResult WriteBlog() => View();
        public IActionResult Content() => View();

    }
}
