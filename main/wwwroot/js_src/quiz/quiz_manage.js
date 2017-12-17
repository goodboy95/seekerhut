window.onload = function () {
    headerMenu();
    $.get("/quizApi/quiz_list", {}, function (resp, stat) {
        var quizList = resp.data;
        for (var i = 0; i < quizList.length; i++) {
            var quizRow = $("#quizRow").clone(true);
            quizRow.find(".quesID").html(quizList[i].quizID);
            quizRow.find(".quesName").html(quizList[i].quizName);
            quizRow.find(".quesURL").html(`http://www.seekerhut.com/quiz/quizpage/${quizList[i].quizID}`);
            $("#quizList").append(quizRow);
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