var path = require('path');
var entryPath = require('./config_module/entry');
const uglify = require('uglifyjs-webpack-plugin');

module.exports = {
    entry: entryPath,
    output: {
        path: path.resolve(__dirname + '/main/wwwroot/js'),
        filename: '[name]',
        chunkFilename: '[id].bundle.js',
    },
    externals: {
        jquery: 'jQuery',
        layui: 'layui',
        //vue: 'vue',
        //vuerouter: 'vue-router'
    },
    resolve: {
        alias: {
            'vue': 'vue/dist/vue.js',
            'vue-router$': 'vue-router/dist/vue-router.common.js'
        }
    },
    module: {
        loaders: [
            {
                test: /\.html$/,
                loader: 'html-loader'
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                loader: 'babel-loader',
            },
            {
                test: /\.vue$/,
                loader: 'vue-loader',
            }
        ]
    },
    plugins: [
        new uglify()
    ]
};
