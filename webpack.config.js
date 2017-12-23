var path = require("path");
var jsSrcRoot = "~/js_src/";
var entryObj = new Object();
var entryPath = [
    "index.js",
    "blog/base.js", "blog/blog.js", "blog/content.js", "blog/writeblog.js",
    "forum/index.js", "forum/postinfo.js", "forum/postlist.js",
    "quiz/answer_view.js", "quiz/create_quiz.js", "quiz/home.js", "quiz/index.js", "quiz/quiz_manage.js", "quiz/quiz.js",
];
entryPath.forEach(jsFileName => {
    entryObj[jsFileName] = path.resolve(jsSrcRoot, jsFileName);
});
module.exports = {
    entry: entryObj,
    output: {
        path: __dirname + "/dist",
        filename: "bundle.js"
    }
}