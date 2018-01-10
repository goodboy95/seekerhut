require("./headerMenu");
var $ = require("jquery");
var quizArr = new Array();

window.onload = function () {
    var quizVue = new Vue({
        el: "#quizList",
        data: {
            quizList: quizArr
        }
    });
    $.get("/quizApi/quiz_list", {}, function (resp, stat) {
        var quizList = resp.data;
        for (var i = 0; i < quizList.length; i++) {
            quizList[i].url = `http://www.seekerhut.com/quiz/quizpage/${quizList[i].quizID}`;
            quizArr.push(quizList[i]);
        }
    });

    document.getElementById("homepage").onclick = function () {
        window.location.href = "/";
    };
};

function EditQuesGroup(editBtn) {
    var quesNum = parseInt($(editBtn).parents(".quiz-row").find(".quesID").html());
    window.location.href = `/quiz/createquiz/${quesNum}`;
}

function GotoQuiz(quizBtn) {
    var quesNum = parseInt($(quizBtn).parents(".quiz-row").find(".quesID").html());
    window.location.href = `/quiz/quizpage/${quesNum}`;
}