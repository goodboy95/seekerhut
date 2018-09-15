using System;
using Microsoft.AspNetCore.Mvc;
using Dao;
using Utils;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace web.Controllers
{
    [Route("page/[controller]")]
    public class HomeController : ViewBaseController
    {
        public HomeController(DwDbContext dbc, ILoggerFactory logFac, IServiceProvider svp) : base(dbc, logFac, svp)
        {
            
        }
        protected override void LoginFail(ActionExecutingContext context)
        {

        }
        [HttpGet("index")]
        public IActionResult Index() 
        {
            ViewBag.CurIP = new HttpParser(HttpContext).GetIPAddr();
            return View();
        } 
         [HttpGet("test")]
        public IActionResult Test() => View();
         [HttpGet("logout")]
        public IActionResult Logout()
        {
            var domain = new HttpParser(HttpContext).GetDomain();
            Response.Cookies.Append("id", "", new CookieOptions {Domain = domain, Expires = DateTime.Now.AddDays(-1.0)});
            Response.Cookies.Append("token", "", new CookieOptions {Domain = domain, Expires = DateTime.Now.AddDays(-1.0)});
            Response.Cookies.Append("username", "", new CookieOptions {Domain = domain, Expires = DateTime.Now.AddDays(-1.0)});
            return Redirect("/");
        }

    }
}
