var layer, form;

function headerMenu() {
    var isLogin = document.getElementById("islogin").value;
    if (isLogin === "true") {
        $("#unlogin-menu").hide();
        $("#logined-menu").show();
    }
    layui.use('element', function () {
        var element = layui.element;
    });
    layui.use('layer', function () {
        layer = layui.layer;
    });
    layui.use('form', function () {
        form = layui.form;
        form.on('submit(login)', function (data) {
            $.post("/homeapi/login", {
                username: data.field.username,
                password: data.field.password,
            }, function (resp, stat) {
                if (resp.code === 0) {
                    alert("logined");
                    location.reload();
                }
                else {
                    alert("login error");
                }
            });
            return false;
        });
        form.on('submit(reg)', function (data) {
            $.post("/homeapi/register", {
                username: data.field.username,
                password: data.field.password,
            }, function (resp, stat) {
                if (resp.code === 0) {
                    alert("registered");
                    location.reload();
                }
                else {
                    alert("register error");
                }
            });
            return false;
        });
    });
    document.getElementById("create").onclick = function () {
        window.location.href = "/quiz/createquiz";
        return false;
    };
    document.getElementById("quiz-manage").onclick = function () {
        window.location.href = "/home/quizmanage";
        return false;
    };
    document.getElementById("answer-manage").onclick = function () {
        window.location.href = "/home/answerview";
        return false;
    };
    document.getElementById("login-link").onclick = function () {
        layer.open({
            type: 1,
            content: $('#login'),
            title: "Sign In"
        });
        return false;
    };
    document.getElementById("reg-link").onclick = function () {
        layer.open({
            type: 1,
            content: $('#register'),
            title: "Sign Up"
        });
        return false;
    };
}