!function(e){function n(r){if(t[r])return t[r].exports;var o=t[r]={i:r,l:!1,exports:{}};return e[r].call(o.exports,o,o.exports,n),o.l=!0,o.exports}var t={};n.m=e,n.c=t,n.d=function(e,t,r){n.o(e,t)||Object.defineProperty(e,t,{configurable:!1,enumerable:!0,get:r})},n.n=function(e){var t=e&&e.__esModule?function(){return e.default}:function(){return e};return n.d(t,"a",t),t},n.o=function(e,n){return Object.prototype.hasOwnProperty.call(e,n)},n.p="",n(n.s=45)}({0:function(e,n){e.exports=jQuery},1:function(e,n){e.exports=layui},45:function(e,n,t){"use strict";var r=(t(0),t(1));window.onload=function(){var e=document.getElementById("blogNum").innerHTML,n=document.getElementById("pageNum").innerHTML;document.getElementById("menu-myblog").classList.add("layui-this"),r.use("laypage",function(){r.laypage.render({elem:"pageBar",count:e,curr:n,limit:15,jump:function(e,n){n||(location.href="/blog/index/"+e.curr)}})})}}});