namespace Domain.Entity
{
    public class ForumSubReplyEntity : BaseEntity
    {
        public long ReplyID { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
    }
}