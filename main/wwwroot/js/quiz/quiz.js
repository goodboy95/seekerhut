!function(e){function t(o){if(n[o])return n[o].exports;var r=n[o]={i:o,l:!1,exports:{}};return e[o].call(r.exports,r,r.exports,t),r.l=!0,r.exports}var n={};t.m=e,t.c=n,t.d=function(e,n,o){t.o(e,n)||Object.defineProperty(e,n,{configurable:!1,enumerable:!0,get:o})},t.n=function(e){var n=e&&e.__esModule?function(){return e.default}:function(){return e};return t.d(n,"a",n),n},t.o=function(e,t){return Object.prototype.hasOwnProperty.call(e,t)},t.p="",t(t.s=91)}({0:function(e,t){e.exports=jQuery},1:function(e,t){e.exports=layui},17:function(e,t,n){e.exports={default:n(18),__esModule:!0}},18:function(e,t,n){var o=n(4),r=o.JSON||(o.JSON={stringify:JSON.stringify});e.exports=function(e){return r.stringify.apply(r,arguments)}},4:function(e,t){var n=e.exports={version:"2.5.3"};"number"==typeof __e&&(__e=n)},91:function(e,t,n){"use strict";function o(e,t){var n,o=new Object,u=t.quesType,s=i[f].options;if("single"===u)n=parseInt(i[f].nextQues);else{var l=parseInt(t.field.answer);n=parseInt(s[l].rel)}return o.quesNo=f,o.answer=t.field.answer,m[c]=o,d[c]=f,n>0?(f=n-1,c++,r()):p.post("/quizApi/answer",{quizID:e,answer:(0,a.default)(m)},function(e,t){alert("You have successfully finished this quiz!"),window.location.href="/"}),!1}function r(){p("#optQuesTitle").html(""),p("#textQuesTitle").html(""),p("#optionArea").html(""),p("#answerArea").val("");var e=i[f],t=e.quesName,n=e.options;if(0===f?p(".prev").hide():p(".prev").show(),p(".ques-number").html("Question "+(c+1)),p(".ques-text").html(""+t),2===parseInt(e.answerType)){p("#optionQues").show(),p("#textQues").hide();for(var o=0;o<n.length;o++){var r=document.getElementById("optAnswer").cloneNode(!0);r.value=o,r.title=n[o].text,0===o&&(r.checked=!0),p("#optionArea").append(p(r)).append("<br />")}u.render("radio")}else p("#optionQues").hide(),p("#textQues").show()}var u,i,s=n(17),a=function(e){return e&&e.__esModule?e:{default:e}}(s),p=n(0),l=n(1),f=0,c=0,d=new Array,m=new Array;window.onload=function(){var e=document.getElementById("quizID").value;l.use("form",function(){u=l.form,u.on("submit(optNext)",function(t){t.quesType="multiple",o(e,t)}),u.on("submit(textNext)",function(t){t.quesType="single",o(e,t)}),u.on("submit(prev)",function(e){f=d[c-1],c--,m.pop(),d.pop(),r()})}),document.getElementById("startQuiz").onclick=function(){p("#intro").hide(),p.get("/quizApi/quiz",{quizID:e},function(e,t){i=JSON.parse(e.data.quizBody),r()})}}}});