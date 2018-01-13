using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class BlogTagRelationEntity : BaseEntity
    {
        public string TagName { get; set; }
        public long UserID { get; set; }
        internal string _blogIDList { get; set; }

        public List<long> BlogIDList
        {
            get { return JsonConvert.DeserializeObject<List<long>>(_blogIDList); }
            set { _blogIDList = JsonConvert.SerializeObject(value); }
        }
    }
}