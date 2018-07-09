<style>

</style>

<template>
     <table class="layui-table">
         <colgroup>
            <col width="150">
            <col width="200">
            <col width="200">
            <col width="200">
            <col>
        </colgroup>
        <thead>
            <tr>
                <th>回复编号</th>
                <th>回复时间</th>
                <th>回复日志</th>
                <th>回复内容</th>
                <th>操作</th>
            </tr> 
        </thead>
        <tbody>
            <tr v-for="reply in replylist">
                <td>{{reply.id}}</td>
                <td>{{reply.create_time}}</td>
                <td>{{reply.blog_title}}</td>
                <td>{{reply.content}}</td>
                <td><button class="layui-btn">删除</button></td>
            </tr>
        </tbody>
     </table>
</template>

<script>
var axios = require('axios');
export default {
    data: function() {
        return {
            replylist: []
        };
    },
    mounted: function() {
        axios.get('/api/admin/my_blog_reply_list', { params: { pageNo: 1, pageSize: 15 } }).then((resp) => {
            this.replylist = resp.data.data.replyList;
        }).catch((err) => console.log(err));
    }
}
</script>

