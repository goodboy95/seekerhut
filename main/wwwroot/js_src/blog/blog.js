var axios = require('axios');
var layui = require('layui');
var Vue = require('vue');

window.onload = function(){
    //SocketConnect(close, SocketReceive, error);
    var bloglist;
    document.getElementById('menu-myblog').classList.add('layui-this');
    layui.use('laypage', function(){
        var laypage = layui.laypage;
        axios.get('/blogapi/blog_list/', { params: { pageNo: 1, pageSize: 15 } }).then(function(resp){
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