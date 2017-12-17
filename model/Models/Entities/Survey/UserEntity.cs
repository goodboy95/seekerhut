using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entity
{
    public class UserEntity : BaseEntity
    {
        public long UserID { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public string Salt { get; set; }
        public string Token { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime LastLoginTime { get; set; }
        public string LastLoginIP { get; set; }
        public int Admin { get; set; }
        public int Exp { get; set; }
        public int Gold { get; set; }
        public int NoticeNum { get; set; }
        internal string _loginIP { get; set; }
        public Dictionary<string, bool> LoginIP
        {
            get { return JsonConvert.DeserializeObject<Dictionary<string, bool>>(_loginIP); }
            set { _loginIP = JsonConvert.SerializeObject(value); }
        }
    }
}
