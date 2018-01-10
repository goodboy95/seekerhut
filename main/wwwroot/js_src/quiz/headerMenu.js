function headerMenu() {
    document.getElementById("create").onclick = function () {
        window.location.href = "/quiz/createquiz";
        return false;
    };
    document.getElementById("quiz-manage").onclick = function () {
        window.location.href = "/home/quizmanage";
        return false;
    };
    document.getElementById("answer-manage").onclick = function () {
        window.location.href = "/home/answerview";
        return false;
    };
    var userdata = new Vue({
        el: "#userdata",
        data: {
            username: "234"
        }
    });
}

module.exports = headerMenu();