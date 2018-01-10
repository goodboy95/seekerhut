require("./headerMenu");
var $ = require("jquery");

var layer;
var answerList;
var quesArr;

window.onload = function () {
    RenderAnswerList();
    layui.use("layer", function () {
        layer = layui.layer;
        $(".viewAnswer").click(function (btn) {
            var answerID = parseInt($(btn.target).parents("#ansRow").find(".ansID").html()) - 1;
            layer.open({
                type: 1,
                title: "Answer List",
                content: $("#answerText"),
                success: function (dom, index) {
                    $("#answerText").empty();
                    var quesArr = JSON.parse(answerList[answerID].quizBody);
                    var answerArr = JSON.parse(answerList[answerID].answerBody);
                    for (var i = 0; i < answerArr.length; i++) {
                        var answerElem = $("#answerElem").clone(true);
                        var ques = quesArr[answerArr[i].quesNo];
                        var answerStr = "";
                        if (parseInt(ques.answerType) >= 2) {
                            var optAnswerNo = parseInt(answerArr[i].answer);
                            answerStr = ques.options[optAnswerNo].text;
                        }
                        else {
                            answerStr = answerArr[i].answer;
                        }
                        answerElem.find("#ques").append(ques.quesName);
                        answerElem.find("#ans").append(answerStr);
                        $("#answerText").append(answerElem);
                    }
                }
            });
        });
    });

    document.getElementById("homepage").onclick = function () {
        window.location.href = "/";
    };
};

function RenderAnswerList() {
    $.get("/quizapi/answer_list", {}, function (resp, stat) {
        answerList = resp.data;
        for (var i = 0; i < answerList.length; i++) {
            var answerRow = $("#ansRow").clone(true);
            answerRow.find(".ansID").html(answerList[i].answerID);
            answerRow.find(".quizName").html(answerList[i].quizName);
            answerRow.find(".ansCreator").html(answerList[i].answerIP);
            $("#answerList").append(answerRow);
        }
    });
}