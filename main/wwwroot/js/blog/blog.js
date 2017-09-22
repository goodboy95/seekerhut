var blogNum = 0;
var laypage;
function getBlogList(pageNo, pageSize){
    $.get("/api/blog/blogList/", {
        authorId: Cookies.get("id"),
        pageNo: 1,
        pageSize: 15
    }, function(resp){
        if (resp.code === 0){
            var blogList = resp.data.blogList,
                blogListHTML = "";
            for(var i=0; i<blogList.length; i++){
                blogListHTML += `<a href=../content?id=${blogList[i].id}>${blogList[i].title}</a><br />`;
            }
            if (resp.data.blogNum != blogNum){
                blogNum = resp.data.blogNum;
                laypage.render({
                    elem: 'pageBar',
                    count: blogNum,
                    limit: 15
                });
            }
            document.getElementById("blogList").innerHTML = blogListHTML;
        }
        else{
            alert(resp.message);
        }
    });
}

window.onload = function(){
    SocketConnect(close, SocketReceive, error);
    document.getElementById("menu-myblog").classList.add("layui-this");
    layui.use('laypage', function(){
        laypage = layui.laypage;
        getBlogList(1, 15);
    });
    
}