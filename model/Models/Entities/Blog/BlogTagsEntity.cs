using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class BlogTagsEntity : BaseEntity
    {
        public string TagName { get; set; }
        public long UserID { get; set; }
        public JsonObject<HashSet<long>> BlogIDList { get; set; }
    }
}