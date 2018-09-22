using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web.Controllers;
using Dao;
using Microsoft.Extensions.Logging;
using web.Api.Controllers;
using Utils;
using Domain.Entity;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web.Controllers
{
    [Route("page/quiz")]
    public class QuizController : ViewBaseController
    {
        private readonly QuizApiController qac;
        public QuizController(DwDbContext dbc, ILoggerFactory logFac, IServiceProvider svp) : base(dbc, logFac, svp) 
        { 
            qac = new QuizApiController(dbc, logFac, svp);
        }
        [HttpGet("index")]
        public IActionResult Index() => View();
        [HttpGet("quiz_manage")]
        public IActionResult QuizManage() => View();
        [HttpGet("answer_view")]
        public IActionResult AnswerView() => View();
        [HttpGet("create_quiz")]
        public IActionResult CreateQuiz([FromRoute]int id)
        {
            ViewBag.quesGroupId = id;
            return View();
        }
        [HttpGet("quiz_page")]
        public IActionResult QuizPage([FromRoute]int id)
        {
            ViewBag.quizID = id;
            var quizResult = (JsonReturn)(qac.GetQuiz(id));
            var quizEntity = (QuizEntity)(quizResult.Data);
            ViewBag.Title = quizEntity.Name;
            ViewBag.Intro = quizEntity.Intro;
            return View();
        }
    }
}
