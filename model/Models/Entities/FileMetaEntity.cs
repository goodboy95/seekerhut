using System;

namespace Domain.Entity{
    public class FileMetaEntity
    {
        public FileMetaEntity()
        {
            FileMetaCreateTime = DateTime.Now;
            FileMetaIsDeleted = false;
        }
        public long FileMetaID { get; set; }
        public DateTime FileMetaCreateTime { get; set; }
        public bool FileMetaIsDeleted { get; set; }
        public string FileMetaName { get; set; }
        public long FileMetaSize { get; set; }
        public string FileMetaSha256 { get; set; }
        public string FileMetaPath { get; set; }
    }
}