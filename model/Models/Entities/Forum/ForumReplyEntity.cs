using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class ForumReplyEntity : BaseEntity
    {
        public long ForumPostID { get; set; }
        public long FatherID { get; set; }  //普通回复为-1,楼中楼为被回复楼层id
        public int AuthorID { get; set; }
        public string Content { get; set; }
    }
}