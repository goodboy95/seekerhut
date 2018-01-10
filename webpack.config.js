var path = require("path");
var entryPath = require("./config_module/entry");
const uglify = require("uglifyjs-webpack-plugin");

module.exports = {
    entry: entryPath,
    output: {
        path: path.resolve(__dirname + "/main/wwwroot/js"),
        filename: "[name]",
        chunkFilename: '[id].bundle.js',
    },
    externals: {
        jquery: "jQuery",
        layui: "layui"
    },
    module: {
        loaders: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                loader: 'babel-loader',
            }
        ]
    },
    plugins: [
        new uglify()
    ]
}
