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
        protected WebSocket webSocket = null;
        protected readonly DwDbContext dbc;
        private ILogger _log;
        public BaseController(DwDbContext dbc, ILoggerFactory logFac)
        {
            this.dbc = dbc;
            _log = logFac.CreateLogger("seekerhut");
        }

        /// <summary>
        /// 从WebSocket中读取数据
        /// </summary>
        /// <param name="context"></param>
        public bool SocketOpen(HttpContext context)
        {
            Console.WriteLine(context.WebSockets.IsWebSocketRequest);
            //判断是否为websocket请求
            if (context.WebSockets.IsWebSocketRequest)
            {
                //接收客户端
                webSocket = context.WebSockets.AcceptWebSocketAsync().Result;
                //启用一个线程处理接收客户端数据
                new Thread(Accept).Start(webSocket);
                while (webSocket.State == WebSocketState.Open) { Thread.Sleep(1000); }
                return true;
            }
            return false;
        }
        public bool SocketSend(string str)
        {
            if (webSocket?.State == WebSocketState.Open)
            {
                webSocket.SendAsync(new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(str)), WebSocketMessageType.Text, true, CancellationToken.None);
                return true;
            }
            else { return false; }
        }
        /// <summary>
        /// 接收客户端数据方法，这里可以根据自己的需求切换功能代码
        /// </summary>
        /// <param name="obj"></param>
        void Accept(object obj)
        {
            var webSocket = obj as WebSocket;
            while (true)
            {
                var acceptArr = new byte[1024];
                var result = webSocket.ReceiveAsync(new ArraySegment<byte>(acceptArr), CancellationToken.None).Result;
                if (result.CloseStatus == WebSocketCloseStatus.NormalClosure) { break; }
                var acceptStr = System.Text.Encoding.UTF8.GetString(acceptArr).Trim(char.MinValue);
                Console.WriteLine("收到信息：" + acceptStr);
            }
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
            if (webSocket?.State != WebSocketState.Open) { SocketOpen(context.HttpContext); }
            //SocketSend();
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