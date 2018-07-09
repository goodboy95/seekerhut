<style>

</style>

<template>
    <table class="layui-table">
        <colgroup>
            <col width="150">
            <col width="200">
            <col width="200">
            <col>
        </colgroup>
        <thead>
            <tr>
                <th>博客编号</th>
                <th>博客标题</th>
                <th>创建时间</th>
                <th>操作</th>
            </tr> 
        </thead>
        <tbody>
            <tr v-for="blog in bloglist">
                <td>{{blog.id}}</td>
                <td>{{blog.title}}</td>
                <td>{{blog.create_time}}</td>
                <td><button class="layui-btn">修改日志</button><button class="layui-btn">删除日志</button></td>
            </tr>
        </tbody>
    </table>
</template>

<script>
var axios = require('axios');
export default {
    data: function(){
        return {
            bloglist: []
        };
    },
    mounted: function(){
        axios.get('/api/admin/my_blog_list', { params: { pageNo: 1, pageSize: 15 } }).then((resp) => {
            this.bloglist = resp.data.data.blogList; //这里的this为vue实例，如果then里想用普通函数，则函数后面应当bind(this)，或在函数前用其他变量暂存this，将变量拿到函数里用
        }).catch((err) => console.log(err));
    },
    methods: {
        demofunc: function(){}
    }
    
}
</script>

