var $ = require("jquery");
var layui = require("layui");
var form;
var currentQues = 0;
var quesCount = 0;
var quizBody;
var quesRoute = new Array();
var answerArr = new Array();

function OnNextClick(quizID, data) {
    var nextQues;
    var ansObj = new Object();
    var quesType = data.quesType;
    var optionsObj = quizBody[currentQues].options;
    if (quesType === "single") {
        nextQues = parseInt(quizBody[currentQues].nextQues);
    }
    else {
        var opt = parseInt(data.field.answer);
        nextQues = parseInt(optionsObj[opt].rel);
    }
    ansObj.quesNo = currentQues;
    ansObj.answer = data.field.answer;
    answerArr[quesCount] = ansObj;
    quesRoute[quesCount] = currentQues;
    if (nextQues > 0) {
        currentQues = nextQues - 1;
        quesCount++;
        RenderQuestion();
    }
    else {
        $.post("/quizApi/answer", {quizID: quizID, answer: JSON.stringify(answerArr)}, function(resp, stat){
            alert("You have successfully finished this quiz!");
            window.location.href = "/";
        });
    }
    return false;
}

function RenderQuestion() {
    $("#optQuesTitle").html("");
    $("#textQuesTitle").html("");
    $("#optionArea").html("");
    $("#answerArea").val("");
    var ques = quizBody[currentQues];
    var quesName = ques.quesName;
    var optionArr = ques.options;
    if (currentQues === 0) {
        $(".prev").hide();
    }
    else {
        $(".prev").show();
    }
    $(".ques-number").html(`Question ${quesCount+1}`);
    $(".ques-text").html(`${quesName}`);
    if (parseInt(ques.answerType) === 2) {
        $("#optionQues").show();
        $("#textQues").hide();
        for (var i = 0; i < optionArr.length; i++) {
            var optionBody = document.getElementById("optAnswer").cloneNode(true);
            optionBody.value = i;
            optionBody.title = optionArr[i].text;
            if (i === 0) {
                optionBody.checked = true;
            }
            $("#optionArea").append($(optionBody)).append("<br />");
        }
        form.render("radio");
    }
    else {
        $("#optionQues").hide();
        $("#textQues").show();
    }
}

window.onload = function () {
    var quizID = document.getElementById("quizID").value;
    layui.use("form", function(){
        form = layui.form;
        form.on("submit(optNext)", function(data){
            data.quesType = "multiple";
            OnNextClick(quizID, data);
        });
        form.on("submit(textNext)", function(data){
            data.quesType = "single";
            OnNextClick(quizID, data);
        });
        form.on("submit(prev)", function(data) {
            currentQues = quesRoute[quesCount-1];
            quesCount--;
            answerArr.pop();
            quesRoute.pop();
            RenderQuestion();
        });
    });
    document.getElementById("startQuiz").onclick = function() {
        $("#intro").hide();
        $.get("/quizApi/quiz", {quizID: quizID}, function(resp, stat){
            quizBody = JSON.parse(resp.data.quizBody);
            RenderQuestion();
        });
    };
};