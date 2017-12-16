var tagArr = new Array();
var layedit, contentBox;
function tagDelete(){
    var tagName = event.srcElement.id;
    var index = tagArr.indexOf(tagName);
    tagArr.splice(index, 1);
    document.getElementById(tagName).remove();
}

window.onload = function(){
    //SocketConnect(close, SocketReceive, error);
    document.getElementById("menu-writeblog").classList.add("layui-this");

    layui.use('layedit', function(){
        layedit = layui.layedit;
        contentBox = layedit.build('content');
    });

    layui.use('layer', function(){
      var layer = layui.layer;
    });

    $.get("/blogapi/tagList/", {userId: Cookies.get("id")}, function(resp){
        if (resp.code === 0){
            var tagList = resp.data.tagList;
            for (var i = 0; i < tagList.length; i++){
                document.getElementById("tagList").innerHTML += `<option value=${tagList[i].tagName}>${tagList[i].tagName}</option>`;
            }
        }
        else{
            alert(resp.msg);
        }
    });

    document.getElementById("createTag").onclick = function(){
        var tagName = document.getElementById("tagText").value;
        document.getElementById("tagList").innerHTML += `<option value=${tagName}>${tagName}</option>`;
        document.getElementById("tagText").value = "";
        alert("标签创建成功！");
    }

    document.getElementById("addTag").onclick = function(){
        layer.open({
            type: 1,
            title: "标签管理",
            content: $("#tagLayer")
        });
    }

    document.getElementById("confirmAddTag").onclick = function(){
        var tagList = document.getElementById("tagList");
        var index = tagList.selectedIndex;
        var tagName = tagList.options[index].text;
        if (tagArr.indexOf(tagName) == -1) {
            document.getElementById("tags").innerHTML += `<a class="tag" id="${tagName}" onclick=tagDelete()>${tagName}</a><br />`;
            tagArr.push(tagName);
            alert("标签添加成功！");
            layer.close(layer.index);
        }
        else {
            alert("该标签已经存在！");
        }
    }
    
    document.getElementById("submit").onclick = function(){
        $.post("/blogapi/blog/", {
            title: document.getElementById("title").value,
            content: layedit.getContent(contentBox),
            tags: tagArr,
            privacy: 0,
            __RequestVerificationToken: $("#token").find("input").val()
        }, function(resp){
            if (resp.code === 0){
                alert("博客上传成功！");
                location.href = "/blog/index/";
            }
            else{
                alert(resp.msg);
            }
        });
    };
};