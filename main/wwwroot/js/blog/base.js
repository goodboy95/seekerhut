var reconnects = 0;
function SocketReceive(data) {
    reconnects = 0;
    socket.send("ok");
    if (data !== "") {
        document.getElementById("notice").innerHTML = JSON.parse(data).BlogNotice;
    }
}

//关闭连接回调
function close() {
    if (reconnects < 5) {
        SocketConnect(close, SocketReceive, error);
        console.log("oh,my...It's closed, reconnecting...");
        reconnects++;
    }
    
    

}

//错误回调
function error(result) {
    if (reconnects < 5) {
        SocketConnect(close, SocketReceive, error);
        console.log("oh,my...It's error, reconnecting...");
        reconnects++;
    }
}