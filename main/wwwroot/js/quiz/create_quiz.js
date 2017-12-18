"use strict";var _regenerator=require("babel-runtime/regenerator"),_regenerator2=_interopRequireDefault(_regenerator),_stringify=require("babel-runtime/core-js/json/stringify"),_stringify2=_interopRequireDefault(_stringify),_promise=require("babel-runtime/core-js/promise"),_promise2=_interopRequireDefault(_promise);function _interopRequireDefault(a){return a&&a.__esModule?a:{default:a}}function _asyncToGenerator(a){return function(){var b=a.apply(this,arguments);return new _promise2.default(function(a,c){function d(e,f){try{var g=b[e](f),h=g.value}catch(a){return void c(a)}return g.done?void a(h):_promise2.default.resolve(h).then(function(a){d("next",a)},function(a){d("throw",a)})}return d("next")})}}var quesNum=0,optionNum=1,curQuesNum=0,questionList=[],form=null,layedit=null,layeditIndex=null,layerIndex=null,answerTypeArr=["Text answer","","Multiple choice"],isAddQues=!1;function AddOption(){var a=$("#quesEditor").find("#o1").clone(!0);a.find("#optionText").val(""),a.find("#relatedQues").val(""),a.find(".ques-num").html("Option "+(optionNum+1)+":"),optionNum++,a.attr("id","o"+optionNum),$("#o"+(optionNum-1)).after(a)}function RemoveOption(){if(!(1>=optionNum)){var a=optionNum;optionNum--,$("#o"+a).remove()}}function ShowAddQuestionDialog(){isAddQues=!0,curQuesNum=quesNum,optionNum=1,layerIndex=layer.open({type:1,content:$("#quesEditor"),title:"Question Editor",area:["500px","500px"],cancel:function(){CleanQuesArea()}})}function EditQues(a){isAddQues=!1;var b=parseInt($(a).parents(".ques-row").attr("id").substring(1));curQuesNum=b-1;var c=questionList[curQuesNum],d=$("#quesEditor");if(d.find("#quesName").val(c.quesName),d.find("#answerType").val(c.answerType),form.render("select"),2<=c.answerType){d.find("#optionInput").show(),optionNum=c.options.length;for(var e,f=0;f<optionNum;f++)if(e=$("#o"+(f+1)),e.find("#optionText").val(c.options[f].text),e.find("#relatedQues").val(c.options[f].rel),f<optionNum-1){var g=$("#quesEditor").find("#o1").clone(!0);g.find("#optionText").val(""),g.find("#relatedQues").val(""),g.find(".ques-num").html("Option "+(f+2)+":"),g.attr("id","o"+(f+2)),$("#o"+(f+1)).after(g)}}else d.find("#textQuesDetail").show(),d.find("#textNextQues").val(c.nextQues);layerIndex=layer.open({type:1,content:$("#quesEditor"),title:"Question Editor",area:["500px","500px"],cancel:function(){CleanQuesArea()}})}function CleanQuesArea(){for(var a=1;a<=optionNum;a++)optionJq=$("#quesEditor").find("#o"+a),1<a?optionJq.remove():(optionJq.find("#optionText").val(""),optionJq.find("#relatedQues").val(""));$("#textNextQues").val(""),$("#quesName").val(""),$("#answerType").val(""),$("#optionInput").hide(),$("#textQuesDetail").hide(),form.render("select"),layer.close(layerIndex)}function AddQuesRowToList(a){quesNum++;var b=$("#questionRow").clone(!0);b.attr("id","q"+(curQuesNum+1)),b.find(".quesID").html(curQuesNum+1),b.find(".quesName").html(a.quesName),b.find(".quesType").html(answerTypeArr[a.answerType]),$("#answerList").append(b)}function SaveQues(a){var b={};if(b.quesName=a.field.quesName,b.answerType=a.field.answerType,2<=parseInt(b.answerType)){b.options=[];for(var c,d=1;d<=optionNum;d++){if(c={},optionJq=$("#quesEditor").find("#o"+d),c.text=optionJq.find("#optionText").val(),c.rel=optionJq.find("#relatedQues").val(),isNaN(parseInt(c.rel)))return void alert("Next question No. must be a number!");b.options.push(c)}}else if(b.nextQues=a.field.nextQues,isNaN(parseInt(b.nextQues)))return void alert("Next question No. must be a number!");if(questionList[curQuesNum]=b,!0==isAddQues)AddQuesRowToList(b);else{var e=$("#q"+(curQuesNum+1));e.find(".quesName").html(b.quesName),e.find(".quesType").html(answerTypeArr[b.answerType])}CleanQuesArea()}window.onload=_asyncToGenerator(_regenerator2.default.mark(function a(){var b,c;return _regenerator2.default.wrap(function(a){for(;;)switch(a.prev=a.next){case 0:return headerMenu(),b=parseInt($("#editid").val()),c=null,a.next=5,layui.use("layedit",function(){return new _promise2.default(function(a){layedit=layui.layedit,layedit.set({uploadImage:{url:"/quizApi/quiz_pic"}}),layeditIndex=layedit.build("introContext"),a()})});case 5:0<b&&$.get("/quizApi/quiz",{quizID:b},function(a){if(0==a.code){$("#quizName").val(a.data.quizName),c=a.data.quizIntro,questionList=JSON.parse(a.data.quizBody);for(var b=0;b<questionList.length;b++)curQuesNum=b,AddQuesRowToList(questionList[b]);layedit.setContent(layeditIndex,c)}}),layui.use("form",function(){form=layui.form,form.on("select(answerType)",function(a){var b=a.value,c=$(a.elem).parents(".layui-form")[0].id;2<=b?($("#optionInput").show(),$("#textQuesDetail").hide()):($("#optionInput").hide(),$("#textQuesDetail").show())}),form.on("submit(saveques)",function(a){SaveQues(a)})}),document.getElementById("addQues").onclick=ShowAddQuestionDialog,document.getElementById("removeQues").onclick=function(){1<=quesNum&&($("#q"+quesNum).remove(),quesNum--,questionList.pop())},document.getElementById("submit").onclick=function(){var a=$("#quizName").val();$.post("/quizApi/quiz",{quizId:parseInt($("#editid").val()),quizName:a,quizIntro:layedit.getContent(layeditIndex),quizJson:(0,_stringify2.default)(questionList)},function(){alert("You've successfully created a quiz!"),window.location.href="/"})};case 10:case"end":return a.stop();}},a,this)}));