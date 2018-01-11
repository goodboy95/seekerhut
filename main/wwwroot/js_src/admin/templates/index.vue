<style>

</style>

<template>
    <div>
        <h1>hello world</h1>
        <p>this is {{people}}</p>
        <button @click="showbox">getit!</button>
    </div>
</template>

<script>
    var layui = require('layui');
    export default {
        data: function(){
            return {
                people: 'hello'
            };
        },
        methods: {
            showbox: function(){
                layui.use('layer', function(){
                    var layer = layui.layer;
                    layer.msg('hello');
                });
            }
        }
    }
</script>
