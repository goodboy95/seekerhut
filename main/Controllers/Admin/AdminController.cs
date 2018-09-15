using System;
using Dao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace web.Controllers
{
    [Route("page/admin")]
    public class AdminController : ViewBaseController
    {
        public AdminController(DwDbContext dbc, ILoggerFactory logFac, IServiceProvider svp) : base(dbc, logFac, svp)
        {
            //_blogApi = new BlogApiController(dbc, logFac, svp);
        }

        [HttpGet("manage_me")]
        public IActionResult ManageMe() => View();
    }
}