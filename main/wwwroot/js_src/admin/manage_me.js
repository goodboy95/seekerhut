var Vue = require('vue');
var VueRouter = require('vue-router');

Vue.use(VueRouter);
Vue.config.debug = true;
Vue.config.devtools = true;

var routes = [
    {
        path: '/', 
        components: require('./templates/index.vue'),
        children: [
            { path: 'blog-manage', components: require('./templates/bloglist.vue') },
            { path: 'blog-recov',components: require('./templates/xb.vue') }
        ]
    },
];

const router = new VueRouter({
    routes
});

new Vue({
    el: '#app',
    router
});