var socket;
var uri = "ws://localhost:8080/";


//发送方法
function SocketSend()
{
    doSend(document.getElementById("sendtxt").value); 
}
//关闭socket
function SocketClose() {
    socket.close();
}

//初始化连接
function SocketConnect(accept, error) {
    //创建websocket,并定义事件回调
    socket = new WebSocket(uri);
    //socket.onopen = function (e) {};
    //socket.onclose = function (e) {};
    socket.onmessage = function (e) { accept(e.data); };
    socket.onerror = function (e) { error(e.data); };
}  
//发送信息
function doSend(message) {
    socket.send(message);
}

/*
//打开连接回调
function open() {
    document.getElementById("message").innerText = "连接打开";
}
//接收数据回调
function accept(result) {
    document.getElementById("output").innerText=result;
}
//关闭连接回调
function close() {
    document.getElementById("message").innerText="连接关闭";
}
*/
//错误回调
function error(result) {
    alert("错误："+result);
}

