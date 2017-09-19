using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class BlogTagRelationEntity : BaseEntity
    {
        public string TagName { get; set; }
        public long UserID { get; set; }
        internal string _blogID { get; set; }

        public List<long> BlogID
        {
            get { return JsonConvert.DeserializeObject<List<long>>(_blogID); }
            set { _blogID = JsonConvert.SerializeObject(value); }
        }
    }
}