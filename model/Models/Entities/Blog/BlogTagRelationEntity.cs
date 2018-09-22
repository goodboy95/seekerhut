using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class BlogTagRelationEntity : BaseEntity
    {
        public long BlogId { get; set; }
        public long TagId { get; set; }
    }
}