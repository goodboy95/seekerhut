using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class ForumReplyEntity : BaseEntity
    {
        public long PostID { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string _subReplyID { get; set; }
        public List<long> SubReplyID 
        {
             get { return JsonConvert.DeserializeObject<List<long>>(_subReplyID); } 
             set { _subReplyID = JsonConvert.SerializeObject(_subReplyID); }
        }
    }
}