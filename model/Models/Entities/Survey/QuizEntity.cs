using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class QuizEntity
    {
        public QuizEntity()
        {
            QuizCreateTime = DateTime.Now;
            QuizIsDeleted = false;
        }
        public int QuizID { get; set; }
        public int QuizCreator { get; set; }
        public DateTime QuizCreateTime { get; set; }
        public bool QuizIsDeleted { get; set; }
        public string QuizName { get; set; }
        public string QuizIntro { get; set; }
        public string QuizPicPath { get; set; }
        public string QuizBody { get; set; }
        internal string _quizLikes { get; set; }
        public List<string> QuizLikes 
        {
             get { return JsonConvert.DeserializeObject<List<string>>(_quizLikes); }
             set { _quizLikes = JsonConvert.SerializeObject(value); }
             }
    }
}