using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class BlogTagRelationEntity
    {
        public BlogTagRelationEntity()
        {
            BtrCreateTime = DateTime.Now;
            BtrIsDeleted = false;
        }
        public long BtrID { get; set; }
        public DateTime BtrCreateTime { get; set; }
        public bool BtrIsDeleted { get; set; }
        public string BtrTagName { get; set; }
        public long BtrUserID { get; set; }
        internal string _blogIDList { get; set; }

        public List<long> BlogIDList
        {
            get { return JsonConvert.DeserializeObject<List<long>>(_blogIDList); }
            set { _blogIDList = JsonConvert.SerializeObject(value); }
        }
    }
}