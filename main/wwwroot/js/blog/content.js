window.onload = function(){
    SocketConnect(close, SocketReceive, error);
    document.getElementById("menu-myblog").classList.add("layui-this");

    var layedit, contentBox, authorID, replyNum,
        queryStr = window.location.href.split("?")[1], 
        id = queryStr.split("=")[1],
        finalPage = false, replies = "";

    layui.use('layedit', function(){
        layedit = layui.layedit;
        contentBox = layedit.build('replyText');
    });
    layui.use('flow', function(){
        var flow = layui.flow;
        flow.load({
            elem: "#replyList",
            done: function(page, next){
                $.get("/api/blog/replylist/", {blogID: id, pageNo: page, pageSize: 5}, function(resp){
                    if (parseInt(resp.code) === 0) { 
                        var replyList = resp.data.replyList;
                        replyNum = resp.data.replyNum;
                        for (var i = 0; i < replyList.length; i++) {
                            var author = replyList[i].user;
                            var content = replyList[i].content;
                            replies += `<p>${author}说 : <br />${content}</p><br />`;
                        }
                        document.getElementById("replyList").innerHTML = replies;
                        next("", page * 5 < replyNum);
                        if (page * 5 >= replyNum) { finalPage = true; }
                    }
                    else { return; }
                });
            }
        });
    });
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

    document.getElementById("replySubmit").onclick = function(){
        var replyContent = layedit.getContent(contentBox);
        var userName = Cookies.get("username");
        $.post("/api/blog/reply/", {
            authorID: authorID,
            blogID: id,
            content: replyContent,
            __RequestVerificationToken: $("#token").find("input").val()
        }, function(resp){
            if (resp.code === 0){
                alert("回复成功！");
                if (!finalPage) { replyNum++; }
                else { document.getElementById("replyList").innerHTML += `<p>${userName}说 : <br />${replyContent}</p><br />`; }
                //layedit.setContent(contentBox, "");
            }
            else{
                alert(resp.msg);
            }
        });
    };
}