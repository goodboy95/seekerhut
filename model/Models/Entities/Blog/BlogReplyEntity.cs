using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class BlogReplyEntity
    {
        public BlogReplyEntity()
        {
            BlogReplyCreateTime = DateTime.Now;
            BlogReplyIsDeleted = false;
        }
        public long BlogReplyID { get; set; }
        public DateTime BlogReplyCreateTime { get; set; }
        public bool BlogReplyIsDeleted { get; set; }
        public long BlogID { get; set; }
        public long BlogReplyAuthorID { get; set; }
        public string BlogReplyContent { get; set; }
        public long BlogReplyFatherID { get; set; }  //对某条回复进行回复时，此处填被回复的回复id。直接回复为-1
        internal string _blogReplyLikeID { get; set; }
        public HashSet<long> BlogReplyLikeID
        {
            get { return JsonConvert.DeserializeObject<HashSet<long>>(_blogReplyLikeID); } 
            set { _blogReplyLikeID = JsonConvert.SerializeObject(_blogReplyLikeID); }
        }
    }
}