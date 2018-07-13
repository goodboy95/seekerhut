using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class BlogReplyEntity : BaseEntity
    {
        public long BlogID { get; set; }
        public long AuthorID { get; set; }
        public string Content { get; set; }
        public long FatherID { get; set; }  //对某条回复进行回复时，此处填被回复的回复id。直接回复为-1
        public JsonObject<HashSet<long>> LikeID { get; set; }
    }
}