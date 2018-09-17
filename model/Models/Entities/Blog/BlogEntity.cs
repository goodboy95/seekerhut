using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class BlogEntity : BaseEntity
    {
        public string Content { get; set; }
        public string Title { get; set; }
        public long AuthorID { get; set; }
        public int Privacy { get; set; }
        public int LikeNum { get; set; }
        public JsonObject<HashSet<long>> LikeID { get; set; }
        public JsonObject<HashSet<long>> DislikeID { get; set; }
        public JsonObject<Dictionary<long, int>> AwardGoldInfo { get; set; }
        public JsonObject<List<long>> VisibleUserID { get; set; }
        public JsonObject<List<FileMetaEntity>> Attachments { get; set; }
        public JsonObject<HashSet<long>> Tags { get; set; }
    }
}