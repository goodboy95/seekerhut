using System;
using Dao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace web.Controllers
{
    [Route("[controller]")]
    public class AdminController : ViewBaseController
    {
        public AdminController(DwDbContext dbc, ILoggerFactory logFac, IServiceProvider svp) : base(dbc, logFac, svp)
        {
            //_blogApi = new BlogApiController(dbc, logFac, svp);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("manage_me")]
        public IActionResult ManageMe()
        {
            return View();
        }
    }
}