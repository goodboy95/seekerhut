var path = require("path");
var entryPath = [
    "index.js",
    "blog/base.js", "blog/blog.js", "blog/content.js", "blog/writeblog.js",
    "forum/index.js", "forum/postinfo.js", "forum/postlist.js",
    "quiz/create_quiz.js", "quiz/manage.js", "quiz/index.js", "quiz/quiz.js",
];
function GetEntryObj() {
    var jsSrcRoot = "main/wwwroot/js_src/";
    var entryObj = new Object();
    return new Promise((resolve, reject) => {
        entryPath.forEach(jsFileName => {
            entryObj[jsFileName] = path.resolve(jsSrcRoot, jsFileName);
        });
        resolve(entryObj);
    });
}

(async function() {
    console.log(await GetEntryObj());
})();
