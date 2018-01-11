var path = require('path');

var entryPath = [
    'index.js',
    'blog/base.js', 'blog/blog.js', 'blog/content.js', 'blog/writeblog.js',
    'forum/index.js', 'forum/postinfo.js', 'forum/postlist.js',
    'quiz/create_quiz.js', 'quiz/index.js', 'quiz/quiz.js', 'quiz/headerMenu.js', 'quiz/answer_view.js', 'quiz/quiz_manage.js',
    'admin/manage_me.js'
    /* '/admin/me_manage.js', '/admin/system_manage.js' */
];
function GetEntryObj() {
    var jsSrcRoot = 'main/wwwroot/js_src/';
    var entryObj = new Object();
    entryPath.forEach(jsFileName => {
        entryObj[jsFileName] = path.resolve(jsSrcRoot, jsFileName);
    });
    return entryObj;
}

module.exports = GetEntryObj();
