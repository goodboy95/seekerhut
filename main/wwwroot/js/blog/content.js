var authorID, pageNum;

window.onload = function(){
    SocketConnect(close, SocketReceive, error);
    document.getElementById("menu-myblog").classList.add("layui-this");
    var layedit, contentBox, 
        queryStr = window.location.href.split("?")[1], 
        id = queryStr.split("=")[1];
    layui.use('layedit', function(){
        layedit = layui.layedit;
        contentBox = layedit.build('replyText');
    });
    layui.use('flow', function(){
        var flow = layui.flow;
        flow.load({
            elem: "#replyList",
            done: function(page, next){
                var data = GetReply(id, page, 10);
                var replyList = data.replyList;
                for (var i = 0; i < replyList.length; i++) {
                    var author = replyList[i].authorID;
                    var content = replyList[i].content;
                    replyText += `<p>${author}说 : <br />${content}</p><br />`;
                }
                document.getElementById("replyList").innerHTML = replyText;
                next("", page <= data.pageNum);
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

function GetReply(blogID, pageNo, pageSize) {
    var replydata;
    $.get("/api/blog/replylist/", {blogID: blogID, pageNo: pageNo, pageSize: pageSize}, function(resp){
        if (resp.code === 0) {
            replydata = resp.data;
        }
        else {
            replydata = null;
        }
    });
    return replydata;
}