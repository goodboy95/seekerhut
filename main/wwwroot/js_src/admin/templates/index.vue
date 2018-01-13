<style>
    .admin-content {
        position: relative;
        margin-left: 250px;
    }
</style>

<template>
    <div>
        <ul class="layui-nav layui-nav-tree layui-nav-side" lay-filter="test">
        <!-- 侧边导航: <ul class="layui-nav layui-nav-tree layui-nav-side"> -->
            <li class="layui-nav-item layui-nav-itemed">
                <a href="javascript:;">探索日志</a>
                <dl class="layui-nav-child">
                    <dd><router-link to="/blog-manage">日志管理</router-link></dd>
                    <dd><router-link to="/blog-recov">自删日志恢复</router-link></dd>
                </dl>
            </li>
            <li class="layui-nav-item">
                <a href="javascript:;">解决方案</a>
                <dl class="layui-nav-child">
                    <dd><a href="">移动模块</a></dd>
                    <dd><a href="">后台模版</a></dd>
                    <dd><a href="">电商平台</a></dd>
                </dl>
            </li>
            <li class="layui-nav-item"><a href="">产品</a></li>
            <li class="layui-nav-item"><a href="">大数据</a></li>
        </ul>
        <router-view class="admin-content"></router-view>
    </div>
</template>

<script>
    var layui = require('layui');
    var element = null, layer = null;
    export default {
        data: function(){
            return {
            };
        },
        methods: {
            showbox: function(){
                layer.msg('hello');
            }
        },
        created: function(){
            layui.use(['element', 'layer'], function(){
                layer = layui.layer;
                element = layui.element;
            });
        }
    }
</script>
