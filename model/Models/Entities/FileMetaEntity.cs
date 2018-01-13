using System;

namespace Domain.Entity{
    public class FileMetaEntity : BaseEntity
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public string Sha256 { get; set; }
        public string Path { get; set; }
    }
}