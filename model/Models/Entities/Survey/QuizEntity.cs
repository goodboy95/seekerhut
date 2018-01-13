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
        internal string _likes { get; set; }
        public List<string> Likes 
        {
             get { return JsonConvert.DeserializeObject<List<string>>(_likes); }
             set { _likes = JsonConvert.SerializeObject(value); }
        }
    }
}