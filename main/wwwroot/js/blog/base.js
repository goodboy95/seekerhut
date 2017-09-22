function SocketReceive(data) {
    socket.send("ok");
    if (data !== "") {
        document.getElementById("notice").innerHTML = JSON.parse(data).BlogNotice;
    }
}

//关闭连接回调
function close() {
    SocketConnect(SocketReceive, error);
    console.log("oh,my...It's closed, reconnecting...");
}

//错误回调
function error(result) {
    SocketConnect(SocketReceive, error);
    console.log("连接异常关闭，开始重连...");
}