using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class BlogEntity
    {
        public BlogEntity()
        {
            BlogCreateTime = DateTime.Now;
            BlogIsDeleted = false;
        }
        public long BlogID { get; set; }
        public DateTime BlogCreateTime { get; set; }
        public bool BlogIsDeleted { get; set; }
        public string BlogContent { get; set; }
        public string BlogTitle { get; set; }
        public long BlogAuthorID { get; set; }
        public int BlogPrivacy { get; set; }
        public int BlogLikeNum { get; set; }
        internal string _blogLikeID { get; set; }
        internal string _blogDislikeID { get; set; }
        internal string _blogAwardGoldInfo { get; set; }
        internal string _blogVisibleUserID { get; set; }
        internal string _blogAttachments { get; set; }
        internal string _blogTags { get; set; }
        public HashSet<long> BlogLikeID
        {
            get{ return JsonConvert.DeserializeObject<HashSet<long>>(_blogLikeID); }
            set{ _blogLikeID = JsonConvert.SerializeObject(value); }
        }
        public HashSet<long> BlogDislikeID
        {
            get{ return JsonConvert.DeserializeObject<HashSet<long>>(_blogDislikeID); }
            set{ _blogDislikeID = JsonConvert.SerializeObject(value); }
        }
        public Dictionary<long, int> BlogAwardGoldInfo
        {
            get{ return JsonConvert.DeserializeObject<Dictionary<long, int>>(_blogAwardGoldInfo); }
            set{ _blogAwardGoldInfo = JsonConvert.SerializeObject(value); }
        }
        public List<FileMetaEntity> BlogAttachments
        {
            get{ return JsonConvert.DeserializeObject<List<FileMetaEntity>>(_blogAttachments); }
            set{ _blogAttachments = JsonConvert.SerializeObject(value); }
        }
        public List<long> BlogVisibleUserID
        {
            get{ return JsonConvert.DeserializeObject<List<long>>(_blogVisibleUserID); }
            set{ _blogVisibleUserID = JsonConvert.SerializeObject(value); }
        }
        public HashSet<string> BlogTags
        {
            get{ return JsonConvert.DeserializeObject<HashSet<string>>(_blogTags); }
            set{ _blogTags = JsonConvert.SerializeObject(value); }
        }
    }
}