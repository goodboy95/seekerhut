using System;

namespace Domain.Entity
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            CreateTime = DateTime.Now;
            IsDeleted = false;
        }
        public DateTime CreateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}