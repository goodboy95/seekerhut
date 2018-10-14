using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Utils
{
    public class WebSocketAccessor
    {
        public HttpContext context;
        public WebSocket webSocket;
        private static object _lock;
        private static Dictionary<long, string> msgDic; //所有等待通过websocket发送的消息
        static WebSocketAccessor()
        {
            msgDic = new Dictionary<long, string>();
            _lock = new object();
        }
        public WebSocketAccessor(HttpContext context)
        {
            this.context = context;
            webSocket = null;
        }

        /// <summary>
        /// 启动WebSocket
        /// </summary>
        /// <param name="context"></param>
        public void SocketOpen()
        {
            //判断是否为websocket请求
            if (context.WebSockets.IsWebSocketRequest)
            {
                var acceptArr = new byte[10];
                var webSocket = context.WebSockets.AcceptWebSocketAsync().Result;
                long uid = Convert.ToInt64(context.Request.Cookies["id"]);
                Array.Clear(acceptArr, 0, acceptArr.Length);
                while (webSocket?.State == WebSocketState.Open)
                {
                    lock(_lock)
                    {
                        var msg = msgDic.ContainsKey(uid) ? msgDic[uid] : null;
                        if (msg != null)
                        {
                            webSocket.SendAsync(new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(msg)),
                                                WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                    Thread.Sleep(10000);
                    if (acceptArr.Where(i => i != 0).FirstOrDefault() == 0) { return; }
                    Array.Clear(acceptArr, 0, acceptArr.Length);
                }
            }
        }

        /// <summary>
        /// 写入新消息提示
        /// </summary>
        /// <param name="receiverID">收信人id</param>
        /// <param name="msgType">提示种类（如博客回复：blogreply，论坛回复：forumreply，私信：privatemsg等）</param>
        /// <returns></returns>
        public bool WriteMsg(long receiverID, MessageType msgType)
        {       
            string msgTypeStr = msgType.ToString();
            Dictionary<string, int> inf;
            lock(_lock)
            {
                if (!msgDic.ContainsKey(receiverID)) 
                {
                    inf = new Dictionary<string, int>();
                    msgDic.Add(receiverID, "");
                }
                else { inf = JsonConvert.DeserializeObject<Dictionary<string, int>>(msgDic[receiverID]); }
                if (inf.ContainsKey(msgTypeStr)) { inf[msgTypeStr]++; }
                else { inf.Add(msgTypeStr, 1); }
                msgDic[receiverID] = JsonConvert.SerializeObject(inf);
            }
            return true;
        }

        public bool ClearMsg(long receiverID, MessageType msgType)
        {
            string msgTypeStr = msgType.ToString();
            lock(_lock)
            {
                if (msgDic.ContainsKey(receiverID))
                {
                    var inf = JsonConvert.DeserializeObject<Dictionary<string, int>>(msgDic[receiverID]);
                    if (inf.ContainsKey(msgTypeStr)) 
                    { 
                        inf[msgTypeStr] = 0;
                        msgDic[receiverID] = JsonConvert.SerializeObject(inf);
                    }
                }
            }
            return true;
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
    }
}