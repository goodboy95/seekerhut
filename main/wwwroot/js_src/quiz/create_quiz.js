require("./headerMenu");
var $ = require("jquery");

var quesNum = 0;
var optionNum = 1;
var curQuesNum = 0;
var questionList = new Array();
var form = null;
var layedit = null;
var layeditIndex = null;
var layerIndex = null;
var answerTypeArr = new Array("Text answer", "", "Multiple choice");
var isAddQues = false;

function AddOption() {
    
    var optionJq = $("#quesEditor").find("#o1").clone(true);
    optionJq.find("#optionText").val("");
    optionJq.find("#relatedQues").val("");
    optionJq.find(".ques-num").html(`Option ${optionNum + 1}:`);
    optionNum++;
    optionJq.attr("id", `o${optionNum}`);
    $(`#o${optionNum - 1}`).after(optionJq);
};

function RemoveOption() {
    if (optionNum <= 1) return;
    var tmpNum = optionNum;
    optionNum--;
    $(`#o${tmpNum}`).remove();
};

function ShowAddQuestionDialog() {
    isAddQues = true;
    curQuesNum = quesNum;
    optionNum = 1;
    layerIndex = layer.open({
        type: 1,
        content: $('#quesEditor'),
        title: "Question Editor",
        area: ['500px', '500px'],
        cancel: function (index, layero) {
            CleanQuesArea();
        }
    });
};

function EditQues(quesDom) {
    isAddQues = false;
    var quesNum = parseInt($(quesDom).parents(".ques-row").attr("id").substring(1));
    curQuesNum = quesNum - 1;
    var quesObj = questionList[curQuesNum];
    var quesField = $('#quesEditor');
    quesField.find("#quesName").val(quesObj.quesName);
    quesField.find("#answerType").val(quesObj.answerType);
    form.render('select');
    if (quesObj.answerType >= 2) {
        quesField.find("#optionInput").show();
        optionNum = quesObj.options.length;
        for (var i = 0; i < optionNum; i++) {
            var opt = $(`#o${i + 1}`);
            opt.find("#optionText").val(quesObj.options[i].text);
            opt.find("#relatedQues").val(quesObj.options[i].rel);
            if (i < optionNum - 1) {
                var optionJq = $("#quesEditor").find("#o1").clone(true);
                optionJq.find("#optionText").val("");
                optionJq.find("#relatedQues").val("");
                optionJq.find(".ques-num").html(`Option ${i + 2}:`);
                optionJq.attr("id", `o${i + 2}`);
                $(`#o${i + 1}`).after(optionJq);
            }
        }
    }
    else {
        quesField.find("#textQuesDetail").show();
        quesField.find("#textNextQues").val(quesObj.nextQues);
    }
    layerIndex = layer.open({
        type: 1,
        content: $('#quesEditor'),
        title: "Question Editor",
        area: ['500px', '500px'],
        cancel: function (index, layero) {
            CleanQuesArea();
        }
    });
};

function CleanQuesArea() {
    for (var i = 1; i <= optionNum; i++) {
        optionJq = $("#quesEditor").find(`#o${i}`);
        if (i > 1) {
            optionJq.remove();
        }
        else {
            optionJq.find("#optionText").val("");
            optionJq.find("#relatedQues").val("");
        }
    }
    $("#textNextQues").val("");
    $("#quesName").val("");
    $("#answerType").val("");
    $("#optionInput").hide();
    $("#textQuesDetail").hide();
    form.render('select');
    layer.close(layerIndex);
};

function AddQuesRowToList(ques) {
    quesNum++;
    var quesRow = $("#questionRow").clone(true);
    quesRow.attr("id", `q${curQuesNum + 1}`);
    quesRow.find(".quesID").html(curQuesNum + 1);
    quesRow.find(".quesName").html(ques.quesName);
    quesRow.find(".quesType").html(answerTypeArr[ques.answerType]);
    $("#answerList").append(quesRow);
};

function SaveQues(data) {
    var ques = new Object();
    ques.quesName = data.field.quesName;
    ques.answerType = data.field.answerType;
    if (parseInt(ques.answerType) >= 2) {
        ques.options = new Array();
        for (var i = 1; i <= optionNum; i++) {
            var option = new Object();
            optionJq = $("#quesEditor").find(`#o${i}`);
            option.text = optionJq.find("#optionText").val();
            option.rel = optionJq.find("#relatedQues").val();
            if (isNaN(parseInt(option.rel))) {
                alert("Next question No. must be a number!");
                return;
            }
            ques.options.push(option);
        }
    }
    else {
        ques.nextQues = data.field.nextQues;
        if (isNaN(parseInt(ques.nextQues))) {
            alert("Next question No. must be a number!");
            return;
        }
    }
    questionList[curQuesNum] = ques;
    if (isAddQues === true) {
        AddQuesRowToList(ques);
    }
    else {
        var quesRow = $(`#q${curQuesNum + 1}`);
        quesRow.find(".quesName").html(ques.quesName);
        quesRow.find(".quesType").html(answerTypeArr[ques.answerType]);
    }
    CleanQuesArea();
};

window.onload = async function () {
    var editQuesgroupId = parseInt($("#editid").val());
    var quizIntro = null;
    await layui.use('layedit', function(){
        return new Promise((resolve, reject) => {
            layedit = layui.layedit;
            layedit.set({
                uploadImage: {
                  url: '/quizApi/quiz_pic'
                }
            });
            layeditIndex = layedit.build('introContext');
            resolve();
        });
    });
    if (editQuesgroupId > 0) {
        $.get("/quizApi/quiz", { quizID: editQuesgroupId }, function(resp, stat){
            if (resp.code == 0) {
                $("#quizName").val(resp.data.quizName);
                quizIntro = resp.data.quizIntro;
                questionList = JSON.parse(resp.data.quizBody);
                for (var i = 0; i < questionList.length; i++) {
                    curQuesNum = i;
                    AddQuesRowToList(questionList[i]);
                }
                layedit.setContent(layeditIndex, quizIntro);
            }
        });
    }

    layui.use("form", function(){
        form = layui.form;
        form.on("select(answerType)", function(data) {
            var choice = data.value;
            var quesId = $(data.elem).parents(".layui-form")[0].id;
            if (choice >= 2) {
                $(`#optionInput`).show();
                $(`#textQuesDetail`).hide();
            }
            else {
                $(`#optionInput`).hide();
                $(`#textQuesDetail`).show();
            }
        });

        form.on('submit(saveques)', function (data) {
            SaveQues(data);
        });
    });

    document.getElementById("addQues").onclick = ShowAddQuestionDialog;

    document.getElementById("removeQues").onclick = function(){
        if (quesNum >= 1) {
            $(`#q${quesNum}`).remove();
            quesNum--;
            questionList.pop();
        }
    };

    document.getElementById("submit").onclick = function(){
        var quizTitle = $("#quizName").val();
        $.post("/quizApi/quiz", { 
            quizId: parseInt($("#editid").val()),
            quizName: quizTitle, 
            quizIntro: layedit.getContent(layeditIndex),
            quizJson: JSON.stringify(questionList) }, function (resp, stat) {
            alert("You've successfully created a quiz!");
            window.location.href = "/";
        });
    };
};