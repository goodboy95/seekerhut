using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class BlogTagRelationEntity : BaseEntity
    {
        public string TagName { get; set; }
        public long UserID { get; set; }
        public JsonObject<List<long>> BlogIDList { get; set; }
    }
}