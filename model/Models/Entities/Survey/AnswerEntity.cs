using System;

namespace Domain.Entity
{
    public class AnswerEntity
    {
        public AnswerEntity()
        {
            AnswerCreateTime = DateTime.Now;
            AnswerIsDeleted = false;
        }
        public int AnswerID { get; set; }
        public DateTime AnswerCreateTime { get; set; }
        public bool AnswerIsDeleted { get; set; }
        public int AnswerCreator { get; set; }
        public string AnswerIP { get; set; }
        public int QuizID { get; set; }
        public string AnswerBody { get; set; }
    }
}