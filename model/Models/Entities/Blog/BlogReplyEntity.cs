using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class BlogReplyEntity : BaseEntity
    {
        public long BlogID { get; set; }
        public long AuthorID { get; set; }
        public string Content { get; set; }
        public long ReReplyID { get; set; }  //对某条回复进行回复时，此处填被回复的回复id
        internal string _likeID { get; set; }
        public HashSet<long> LikeID
        {
            get { return JsonConvert.DeserializeObject<HashSet<long>>(_likeID); } 
            set { _likeID = JsonConvert.SerializeObject(_likeID); }
        }
    }
}