using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class ForumPostEntity : BaseEntity
    {
        public long ForumID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int ViewLevel { get; set; }
        public int ReplyLevel { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        internal string _replyID { get; set; }
        public List<long> ReplyID 
        {
            get { return JsonConvert.DeserializeObject<List<long>>(_replyID); }
            set { _replyID = JsonConvert.SerializeObject(value); }
        }

        
    }
}