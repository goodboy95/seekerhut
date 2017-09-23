var socket = null;

//发送方法
function SocketSend()
{
    doSend(document.getElementById("sendtxt").value); 
}


//初始化连接
function SocketConnect(close, accept, error) {
    var host = window.location.host;
    if (!host.includes("localhost")) { host = "www." + host; }
    var uri = "ws://" + host;
    console.log(uri);
    //创建websocket,并定义事件回调
    socket = new WebSocket(uri);
    //socket.onopen = function (e) {};
    socket.onclose = function (e) { close(); };
    socket.onmessage = function (e) { accept(e.data); };
    socket.onerror = function (e) { error(e.data); };
}  
//发送信息
function doSend(message) {
    socket.send(message);
}



