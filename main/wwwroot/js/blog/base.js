var reconnects = 0;
function SocketReceive(data) {
    reconnects = 0;
    socket.send("ok");
    if (data !== "") {
        var jdata = JSON.parse(data);
        document.getElementById("notice").innerHTML = jdata["BlogReply"] > 0 ? jdata["BlogReply"] : 0;
    }
}
