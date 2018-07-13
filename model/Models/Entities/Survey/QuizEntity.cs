using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class QuizEntity : BaseEntity
    {
        public int CreatorID { get; set; }
        public string Name { get; set; }
        public string Intro { get; set; }
        public string PicPath { get; set; }
        public string Body { get; set; }
        public JsonObject<List<string>> Likes { get; set; }
    }
}