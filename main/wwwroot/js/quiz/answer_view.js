!function(e){function n(r){if(t[r])return t[r].exports;var a=t[r]={i:r,l:!1,exports:{}};return e[r].call(a.exports,a,a.exports,n),a.l=!0,a.exports}var t={};n.m=e,n.c=t,n.d=function(e,t,r){n.o(e,t)||Object.defineProperty(e,t,{configurable:!1,enumerable:!0,get:r})},n.n=function(e){var t=e&&e.__esModule?function(){return e.default}:function(){return e};return n.d(t,"a",t),t},n.o=function(e,n){return Object.prototype.hasOwnProperty.call(e,n)},n.p="",n(n.s=89)}({0:function(e,n){e.exports=jQuery},5:function(e,n,t){"use strict";e.exports=function(){document.getElementById("create").onclick=function(){return window.location.href="/quiz/createquiz",!1},document.getElementById("quiz-manage").onclick=function(){return window.location.href="/home/quizmanage",!1},document.getElementById("answer-manage").onclick=function(){return window.location.href="/home/answerview",!1};new Vue({el:"#userdata",data:{username:"234"}})}()},89:function(e,n,t){"use strict";function r(){i.get("/quizapi/answer_list",{},function(e,n){o=e.data;for(var t=0;t<o.length;t++){var r=i("#ansRow").clone(!0);r.find(".ansID").html(o[t].answerID),r.find(".quizName").html(o[t].quizName),r.find(".ansCreator").html(o[t].answerIP),i("#answerList").append(r)}})}t(5);var a,o,i=t(0);window.onload=function(){r(),layui.use("layer",function(){a=layui.layer,i(".viewAnswer").click(function(e){var n=parseInt(i(e.target).parents("#ansRow").find(".ansID").html())-1;a.open({type:1,title:"Answer List",content:i("#answerText"),success:function(e,t){i("#answerText").empty();for(var r=JSON.parse(o[n].quizBody),a=JSON.parse(o[n].answerBody),u=0;u<a.length;u++){var s=i("#answerElem").clone(!0),c=r[a[u].quesNo],l="";if(parseInt(c.answerType)>=2){var f=parseInt(a[u].answer);l=c.options[f].text}else l=a[u].answer;s.find("#ques").append(c.quesName),s.find("#ans").append(l),i("#answerText").append(s)}}})})}),document.getElementById("homepage").onclick=function(){window.location.href="/"}}}});