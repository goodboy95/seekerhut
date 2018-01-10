var $ = require("jquery");
var layui = require("layui");

var replyArr = new Array();

window.onload = function(){
    //SocketConnect(close, SocketReceive, error);
    var replyVue = new Vue({
        el: "#replyList",
        data: { replies: replyArr } 
    });
    document.getElementById("menu-myblog").classList.add("layui-this");

    var layedit, contentBox, replyNum,
        id = document.getElementById("blogID").innerHTML,
        finalPage = false, replies = "";

    layui.use("layedit", function(){
        layedit = layui.layedit;
        contentBox = layedit.build("replyText");
    });
    layui.use("flow", function(){
        var flow = layui.flow;
        flow.load({
            elem: "#replyList",
            done: function(page, next){
                $.get("/blogapi/replylist/", {blogID: id, pageNo: page, pageSize: 5}, function(resp){
                    if (parseInt(resp.code) === 0) { 
                        replyNum = resp.data.ReplyNum;
                        var replyList = resp.data.ReplyList;
                        for (var i = 0; i < replyList.length; i++) {
                            var author = replyList[i].author;
                            var content = replyList[i].content;
                            var replyObj = {author: author, content: content};
                            replyArr.push(replyObj)
                        }
                        next("", page * 5 < replyNum);
                        if (page * 5 >= replyNum) { finalPage = true; }
                    }
                    else { return; }
                });
            }
        });
    });

    document.getElementById("replySubmit").onclick = function(){
        var replyContent = layedit.getContent(contentBox);
        var userName = Cookies.get("username");
        var authorID = document.getElementById("authorID").innerHTML;
        $.post("/blogapi/reply/", {
            blogAuthorID: authorID,
            blogID: id,
            content: replyContent,
            __RequestVerificationToken: $("#token").find("input").val()
        }, function(resp){
            if (resp.code === 0){
                alert("回复成功！");
                if (!finalPage) { replyNum++; }
                else 
                { 
                    var replyObj = {author: userName, content: replyContent};
                    replyArr.push(replyObj);
                }
                //layedit.setContent(contentBox, "");
            }
            else{
                alert(resp.msg);
            }
        });
    };
}