using System;

namespace Domain.Entity{
    public class PictureEntity : BaseEntity
    {
        public string PicName { get; set; }
        public long PicSize { get; set; }
        public string PicSha256 { get; set; }
        public string PicPath { get; set; }
    }
}