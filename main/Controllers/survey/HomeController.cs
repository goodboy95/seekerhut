using System;
using Microsoft.AspNetCore.Mvc;
using Dao;
using Utils;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using web.Api.Controllers;
using Newtonsoft.Json.Linq;
using System.Linq;
using Domain.Entity;

namespace web.Controllers
{
    public class HomeController : ViewBaseController
    {
        protected override void LoginFail(ActionExecutingContext context)
        {
        }
        public HomeController(DwDbContext dbc, ILoggerFactory logFac, IServiceProvider svp) : base(dbc, logFac, svp)
        {
        }
        public IActionResult Index() => View();
        public IActionResult QuizManage() => View();
        public IActionResult AnswerView() => View();
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
