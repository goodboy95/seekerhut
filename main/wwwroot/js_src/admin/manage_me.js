var Vue = require('vue');
var VueRouter = require('vue-router');
var AdminIndex = require('./templates/index.vue');

Vue.use(VueRouter);
Vue.config.debug = true;
Vue.config.devtools = true;

var routes = [
    {path: '/', components: AdminIndex},
    // {path: '/list', components: require('./components/list')},
    // {path: '*', components: require('./components/notFound')}
];

const router = new VueRouter({
    routes
});

new Vue({
    el: '#app',
    router
});