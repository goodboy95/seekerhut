using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class AttachObj
    {
        public string FileName;
        public string Path;
        public long size;
    }
    public class BlogEntity : BaseEntity
    {
        public string Content { get; set; }
        internal long ReplyNum { get; set; }
        public string Title { get; set; }
        public long AuthorID { get; set; }
        public int Privacy { get; set; }
        public int LikeNum { get; set; }
        internal string _likeID { get; set; }
        internal string _dislikeID { get; set; }
        internal string _awardGoldInfo { get; set; }
        internal string _visibleUserID { get; set; }
        internal string _attachments { get; set; }
        internal string _tags { get; set; }
        public HashSet<long> LikeID
        {
            get{ return JsonConvert.DeserializeObject<HashSet<long>>(_likeID); }
            set{ _likeID = JsonConvert.SerializeObject(value); }
        }
        public HashSet<long> DislikeID
        {
            get{ return JsonConvert.DeserializeObject<HashSet<long>>(_dislikeID); }
            set{ _dislikeID = JsonConvert.SerializeObject(value); }
        }
        public Dictionary<long, int> AwardGoldInfo
        {
            get{ return JsonConvert.DeserializeObject<Dictionary<long, int>>(_awardGoldInfo); }
            set{ _awardGoldInfo = JsonConvert.SerializeObject(value); }
        }
        public List<AttachObj> Attachments
        {
            get{ return JsonConvert.DeserializeObject<List<AttachObj>>(_attachments); }
            set{ _attachments = JsonConvert.SerializeObject(value); }
        }
        public List<long> VisibleUserID
        {
            get{ return JsonConvert.DeserializeObject<List<long>>(_visibleUserID); }
            set{ _visibleUserID = JsonConvert.SerializeObject(value); }
        }
        
        public HashSet<string> Tags
        {
            get{ return JsonConvert.DeserializeObject<HashSet<string>>(_tags); }
            set{ _tags = JsonConvert.SerializeObject(value); }
        }
    }
}