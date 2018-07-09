<style>

</style>

<template>
    <div class="layui-tab">
        <ul class="layui-tab-title">
            <li class="layui-this">我的回复</li>
            <li>用户管理</li>
            <li>权限分配</li>
            <li>商品管理</li>
            <li>订单管理</li>
        </ul>
        <div class="layui-tab-content">
            <router-view></router-view>
        </div>
    </div>
</template>

<script>
var layui = require('layui');
var tabPath = ['myreply', 'replyme'];

export default {
    created: function() {
        var vueObj = this;
        layui.use('element', function(){
            var element = layui.element;
            vueObj.$router.push({path: 'myreply', append: true});
            element.on('tab', function(data){
                vueObj.$router.push({path: tabPath[data.index]});
            });
        }.bind(this));
    }
}
</script>

