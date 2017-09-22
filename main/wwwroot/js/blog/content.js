var authorID;

window.onload = function(){
    SocketConnect(close, SocketReceive, error);
    document.getElementById("menu-myblog").classList.add("layui-this");
    var layedit, contentBox, 
        queryStr = window.location.href.split("?")[1], 
        id = queryStr.split("=")[1];
    layui.use('layedit', function(){
        layedit = layui.layedit;
        contentBox = layedit.build('replyText');
    })
    $.get("/api/blog/blog/", {id: id}, function(resp){
        if (resp.code == 0){
            var blog = resp.data.blog;
            authorID = blog.authorID;
            document.getElementById("title").innerHTML = blog.title;
            document.getElementById("author").innerHTML = resp.data.authorName;
            document.getElementById("createTime").innerHTML = resp.data.createTime;
            document.getElementById("content").innerHTML = blog.content;
        }
        else{
            alert(resp.msg);
        }
    });
    //TODO: 回复列表要改为点击按钮后再加载
    $.get("/api/blog/replylist/", {blogID: id}, function(resp){
        if (resp.code === 0) {
            var replyText = "", 
                replyList = resp.data.replyList;
            for (var i = 0; i < replyList.length; i++) {
                var author = replyList[i].authorID;
                var content = replyList[i].content;
                replyText += `<p>${author}说 : <br />${content}</p><br />`;
            }
            document.getElementById("replyList").innerHTML = replyText;
        }
        else {
            alert(resp.msg);
        }
    });

    document.getElementById("replySubmit").onclick = function(){
        $.post("/api/blog/reply/", {
            authorID: authorID,
            blogID: id,
            content: layedit.getContent(contentBox),
            __RequestVerificationToken: $("#token").find("input").val()
        }, function(resp){
            if (resp.code === 0){
                alert("回复成功！");
                location.reload();
            }
            else{
                alert(resp.msg);
            }
        });
    };
}