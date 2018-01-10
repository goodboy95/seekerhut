var $ = require("jquery");
var layui = require("layui");

window.onload = function(){
    //SocketConnect(close, SocketReceive, error);
    var blogNum = document.getElementById("blogNum").innerHTML;
    var pageNum = document.getElementById("pageNum").innerHTML;
    document.getElementById("menu-myblog").classList.add("layui-this");
    layui.use("laypage", function(){
        var laypage = layui.laypage;
        laypage.render({
            elem: "pageBar",
            count: blogNum,
            curr: pageNum,
            limit: 15,
            jump: function(obj, first) {
                if (!first)
                    location.href = `/blog/index/${obj.curr}`;
            }
        });
    });
}