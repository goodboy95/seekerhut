using System;

namespace Domain.Entity
{
    public class AnswerEntity : BaseEntity
    {
        public int CreatorID { get; set; }
        public string SourceIP { get; set; }
        public int QuizID { get; set; }
        public string Body { get; set; }
    }
}