using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using model;
using Newtonsoft.Json.Linq;

namespace Utils
{
    public class WebSocketAccessor
    {
        public HttpContext context;
        public WebSocket webSocket;
        private static Dictionary<long, Stack<string>> msgDic;
        private static bool msgCanAccess; //msg被读写时加此锁(置为false)，防止访问（毕竟对于stack而言，读和写没啥差别）
        static WebSocketAccessor()
        {
            msgDic = new Dictionary<long, Stack<string>>();
            msgCanAccess = true;
        }
        public WebSocketAccessor()
        {
            webSocket = null;
        }

        /// <summary>
        /// 启动WebSocket
        /// </summary>
        /// <param name="context"></param>
        public bool SocketOpen(HttpContext context)
        {
            this.context = context;
            //判断是否为websocket请求
            if (context.WebSockets.IsWebSocketRequest)
            {
                //接收客户端
                webSocket = context.WebSockets.AcceptWebSocketAsync().Result;
                //启用线程发送接收客户端数据
                //new Thread(Accept).Start(webSocket);
                new Thread(MsgSend).Start(webSocket);
                while (webSocket?.State == WebSocketState.Open) 
                {
                     Thread.Sleep(10000); 
                }
                return true;
            }
            return false;
        }

        public async Task<bool> WriteMsg(long receiverID, string msg)
        {
            await Task.Run((Action)ensureMsgWritable);          
            msgCanAccess = false;
            if (!msgDic.ContainsKey(receiverID)) { msgDic.Add(receiverID, new Stack<string>()); }
            msgDic[receiverID].Push(msg);
            msgCanAccess = true;
            return true;
        }

        /// <summary>
        /// 独立线程，通过websocket发送数据
        /// </summary>
        /// <param name="obj"></param>
        void MsgSend(object obj)
        {
            var webSocket = obj as WebSocket;
            var acceptArr = new byte[1024];
            long.TryParse(context.Request.Cookies["id"], out long uid);
            while (webSocket?.State == WebSocketState.Open)
            {
                if (!msgCanAccess) { Thread.Sleep(10); }
                msgCanAccess = false;
                var msgStack = msgDic.ContainsKey(uid) ? msgDic[uid] : null;
                while (msgStack != null && msgStack.Count > 0)
                {
                    var msg = msgStack.Pop();
                    webSocket.SendAsync(new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(msg)),
                                        WebSocketMessageType.Text, true, CancellationToken.None);
                }
                msgCanAccess = true;
                Thread.Sleep(10000);
            }
        }

        /// <summary>
        /// 通过websocket发送数据
        /// </summary>
        /// <param name="str">发送的字符串</param>
        /// <returns></returns>
        public bool SocketSend(string str)
        {
            if (webSocket?.State == WebSocketState.Open)
            {
                webSocket.SendAsync(new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(str)), 
                                    WebSocketMessageType.Text, true, CancellationToken.None);
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// 独立线程，通过websocket接收数据
        /// </summary>
        /// <param name="obj"></param>
        void Accept(object obj)
        {
            var webSocket = obj as WebSocket;
            var acceptArr = new byte[1024];
            while (webSocket?.State == WebSocketState.Open)
            {
                var result = webSocket.ReceiveAsync(new ArraySegment<byte>(acceptArr), CancellationToken.None).Result;
                var acceptStr = Encoding.UTF8.GetString(acceptArr).Trim(char.MinValue);
                if (webSocket?.State == WebSocketState.Open) 
                {
                    
                     /*msgDic.Add(1, acceptStr); Console.WriteLine(acceptStr);*/ 
                }
            }
        }

        void ensureMsgWritable()
        {
            while (!msgCanAccess) { Thread.Sleep(10); }
        }
    }
}