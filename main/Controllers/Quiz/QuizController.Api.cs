using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dao;
using Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Domain.Entity;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.IO;
using System.Web;

namespace web.Api.Controllers
{
    [Route("[controller]")]
    public class QuizApiController : ApiBaseController
    {
        public QuizApiController(DwDbContext dbc, ILoggerFactory logFac, IServiceProvider svp) : base(dbc, logFac, svp)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("quiz_pic")]
        public ActionResult SaveQuizPic()
        {
            const string picBasePath = "/home/duoyi/file/pics";
            if (!Directory.Exists(picBasePath)) { Directory.CreateDirectory(picBasePath); }
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null) { return JsonReturn.ReturnFail("File is not exist!"); }
            var fileNameSplit = file.FileName.Split(".");
            var splitCount = fileNameSplit.Count();
            var fileExt = fileNameSplit[splitCount - 1];
            var fileNameBody = file.FileName.Replace($".{fileExt}", "");
            var newBody = fileNameBody;
            int duplicate = 0;
            string filePath = $"{picBasePath}/{newBody}.{fileExt}";
            if (System.IO.File.Exists(filePath))
            {
                newBody = $"{fileNameBody}{duplicate}";
                filePath = $"{picBasePath}/{newBody}.{fileExt}";
                duplicate++;
            }
            FileStream fs = System.IO.File.Create(filePath);
            file.CopyTo(fs);
            fs.Flush();
            fs.Dispose();
            return JsonReturn.ReturnSuccess(data: new JObject(){ ["src"] = $"/file/pics/{newBody}.{fileExt}", ["title"] = "uploadpic" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="quizName"></param>
        /// <param name="quizIntro"></param>
        /// <param name="quizJson"></param>
        /// <returns></returns>
        [HttpPost("quiz")]
        public ActionResult SaveQuiz([FromForm]int quizId, [FromForm]string quizName, [FromForm]string quizIntro, [FromForm]string quizJson)
        {
            var quizObj = new QuizEntity();
            int.TryParse(Request.Cookies["id"], out int creator);
            quizObj.Body = quizJson;
            quizObj.Name = quizName;
            quizObj.Intro = quizIntro;
            quizObj.CreatorID = creator;
            quizObj.Likes = new List<string>();
            dbc.Quiz.Add(quizObj);
            if (quizId > 0)
            {
                var prevQues = dbc.Quiz.Find(quizId);
                prevQues.IsDeleted = true;
                dbc.Quiz.Update(prevQues);
            }
            dbc.SaveChanges();
            return JsonReturn.ReturnSuccess();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizID"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        [HttpPost("answer")]
        public ActionResult SaveAnswer([FromForm]int quizID, [FromForm]string answer)
        {
            var answerObj = new AnswerEntity();
            int.TryParse(Request.Cookies["id"], out int creator);  //暂时没有用户名
            answerObj.CreatorID = creator;
            answerObj.SourceIP = new HttpParser(HttpContext).GetIPAddr();
            answerObj.QuizID = quizID;
            answerObj.Body = answer;
            dbc.Answer.Add(answerObj);
            dbc.SaveChanges();
            return JsonReturn.ReturnSuccess();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizID"></param>
        /// <returns></returns>
        [HttpGet("quiz")]
        public ActionResult GetQuiz([FromQuery]int quizID)
        {
            return JsonReturn.ReturnSuccess(dbc.Quiz.Find(quizID));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        [HttpGet("answer")]
        public ActionResult GetAnswer([FromQuery]int answerID)
        {
            var answerEntity = dbc.Answer.Find(answerID);
            var relQuizID = answerEntity.QuizID;
            var answerBody = answerEntity.Body;
            var quizBody = dbc.Quiz.Find(relQuizID).Body;
            var result = new JObject(){ ["quizBody"] = quizBody, ["answerBody"] = answerBody };
            return JsonReturn.ReturnSuccess(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("quiz_list")]
        public ActionResult GetQuizList()
        {
            var quizList = from al in dbc.Quiz where al.IsDeleted == false select al;
            return JsonReturn.ReturnSuccess(quizList);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("answer_list")]
        public ActionResult GetAnswerList()
        {
            var quizList = from sl in dbc.Quiz select sl;
            var answerList = from al in dbc.Answer
            join ql in quizList on al.QuizID equals ql.ID
            select new { AnswerID = al.ID, AnswerBody = al.Body, AnswerIP = al.SourceIP,
                QuizName = ql.Name, QuizBody = ql.Body };
            return JsonReturn.ReturnSuccess(answerList);
        }

        [HttpGet("test")]
        public ActionResult Test()
        {
            var x = new JObject();
            x["12"] = 5;
            return JsonReturn.ReturnSuccess(x);
        }
    }
}