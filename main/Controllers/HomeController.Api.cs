using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Dao;
using System.Text;
using Utils;
using Domain.Entity;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace web.Api.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : ApiBaseController
    {
        public HomeController(DwDbContext dbc, ILoggerFactory logFac) : base(dbc, logFac)
        {
        }
        protected override void LoginFail(ActionExecutingContext context)
        {
        }
        public string HashStr(string src)
        {
            var sha2 = SHA256.Create();
            byte[] hashByte = sha2.ComputeHash(Encoding.Unicode.GetBytes(src));
            string passHash = "";
            for (int i = 0;i < hashByte.Length; i++)
            {
                passHash += hashByte[i].ToString("x2");
            }
            return passHash;
        }
        [HttpPut("register")]
        public JsonReturn Register(string username, string password)
        {
            username = HTMLEntity.XSSConvert(username);
            string salt = HashStr(username + DateTime.Now.ToString());
            string passHash = HashStr(salt + password + salt + username);
            string ip = new HttpParser(HttpContext).GetIPAddr();
            var loginIPDic = new Dictionary<string, bool>();
            loginIPDic.Add(ip, true);
            UserEntity u = new UserEntity{Name = username, Pass = passHash, Salt = salt, LoginIP = loginIPDic};
            dbc.User.Add(u);
            dbc.SaveChanges();
            return JsonReturn.ReturnSuccess();
        }
        [HttpPost("login")]
        public JsonReturn Login(string username, string password)
        {
            username = HTMLEntity.XSSConvert(username);
            var domain = new HttpParser(HttpContext).GetDomain();
            UserEntity u = (from lu in dbc.User where lu.Name == username select lu).FirstOrDefault();
            if (u == null) { return JsonReturn.ReturnFail(-1, "该用户不存在！"); }
            string salt = u.Salt;
            string passHash = HashStr(salt + password + salt + username);
            if (u.Pass != passHash) { return JsonReturn.ReturnFail(-2, "密码错误！"); }
            else 
            {
                if (u.Token == null) 
                {
                    string token = HashStr(password + DateTime.Now.ToString() + username);
                    u.Token = token;
                    u.ExpireTime = DateTime.Now.AddMonths(1);
                    dbc.SaveChanges();
                }
                Response.Cookies.Append("username", username, new CookieOptions { Domain = domain, Expires = DateTime.Now.AddMonths(1) });
                Response.Cookies.Append("token", u.Token, new CookieOptions { Domain = domain, Expires = DateTime.Now.AddMonths(1) });
                Response.Cookies.Append("id", u.UserID.ToString(), new CookieOptions { Domain = domain, Expires = DateTime.Now.AddMonths(1) });
                return JsonReturn.ReturnSuccess();
            }
        }
    }
}