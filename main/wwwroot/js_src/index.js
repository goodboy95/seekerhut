var $ = require("jquery");
var layui = require("layui");

layui.use("layer", function(){
    var layer = layui.layer;

    window.onload = function(){
        var username = Cookies.get("username");
        if (username != undefined){
            $("#reg").hide();
            $("#login").hide();
            $("#userdata").show();
            var userid = Cookies.get("id");
            document.getElementById("user").innerHTML += `<a href="/user/userinfo?id=${userid}">${username}</a>`;
        }
        document.getElementById("regWindow").onclick = function(){
            layer.open({
                type: 1,
                title: "注册",
                content: $("#reg")
            });
        };
        document.getElementById("regSubmit").onclick = function(){
            $.post("/homeapi/register/", {
                __RequestVerificationToken: $("#reg").find("#token").find("input").val(),
                username: $("#reg").find("#reg-username").val(),
                password: md5($("#reg").find("#reg-password").val())
            }, function(resp){
                if (resp.code === 0){
                    alert("用户注册成功！");
                    location.reload();
                }
                else{
                    alert(resp.msg);
                }
            });
        };
        document.getElementById("loginSubmit").onclick = function(){
            $.post("/homeapi/login/", {
                __RequestVerificationToken: $("#login").find("#token").find("input").val(),
                username: $("#login").find("#login-username").val(),
                password: md5($("#login").find("#login-password").val())
            }, function(resp){
                if (resp.code === 0){
                    alert("登录成功！");
                    location.reload();
                }
                else{
                    alert(resp.msg);
                }
            });
        };
    };
});  

