using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class ForumEntity : BaseEntity
    {
        public long ForumID { get; set; }
        public string Name { get; set; }
        public int CreatorID { get; set; }
        public int ViewLevel { get; set; }
        public int PostLevel { get; set; }
        public string Admin { get; set; }
        public int Status { get; set; }
        internal string _replyID { get; set; }
        public List<long> ReplyID
        {
            get{ return JsonConvert.DeserializeObject<List<long>>(_replyID); }
            set{ _replyID = JsonConvert.SerializeObject(value); }
        }
    }
}