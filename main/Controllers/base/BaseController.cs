using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dao;
using Domain.Entity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Utils;
using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;
using System.Threading;

namespace web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected WebSocketAccessor wsa = null;
        protected readonly DwDbContext dbc;
        private ILogger _log;
        public BaseController(DwDbContext dbc, ILoggerFactory logFac)
        {
            this.dbc = dbc;
            _log = logFac.CreateLogger("seekerhut");
        }


        public string GetIPAddr()
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
        protected virtual void LoginFail(ActionExecutingContext context) {}
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            long userID = Convert.ToInt64(Request.Cookies["id"]);
            string token = Request.Cookies["token"];
            var curUser = (from u in dbc.User where u.ID == userID select new {u.Token, u.ExpireTime}).FirstOrDefault();
            string realToken = curUser?.Token;
            var tokenTime = curUser?.ExpireTime;
            if (wsa == null) { wsa = new WebSocketAccessor(context.HttpContext); }
            if (wsa.webSocket?.State != WebSocketState.Open) { wsa.SocketOpen(); }
            if (curUser == null || token == null || realToken == null || token != realToken || tokenTime < DateTime.Now)
            {
                LoginFail(context);
            }
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }
        public int GetAdminLevel()
        {
            string username = Request.Cookies["username"];
            int adminLevel = (from user in dbc.User where user.Name == username select user.Admin).FirstOrDefault();
            return adminLevel;
        }
    }
}