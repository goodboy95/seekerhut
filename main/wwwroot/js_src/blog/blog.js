var axios = require('axios');
var layui = require('layui');
var Vue = require('vue');
var Cookies = require('cookies');

window.onload = function(){
    //SocketConnect(close, SocketReceive, error);
    var bloglist;
    document.getElementById('menu-myblog').classList.add('layui-this');
    layui.use('laypage', function(){
        var laypage = layui.laypage;
        axios.get('/api/blog/blog_list/', { params: { authorId: Cookies.get('id'), pageNo: 1, pageSize: 15 } }).then(function(resp){
            bloglist = resp.data.data.blogList;
            new Vue({
                el: '#blogArea',
                data: {
                    bloglist: bloglist
                }
            });
        }).catch(function(err){
            console.error(err);
        });
        
    });
};