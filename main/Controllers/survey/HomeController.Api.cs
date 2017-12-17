using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utils;
using System.Security.Cryptography;
using Dao;
using Microsoft.Extensions.Logging;
using web.Api.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Domain.Entity;
using Microsoft.AspNetCore.Http;
using static Utils.UtilClass;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace simpleproj.Controllers
{
    [Route("[controller]")]
    public class HomeApiController : ApiBaseController
    {
        public HomeApiController(DwDbContext dbc, ILoggerFactory logFac, IServiceProvider svp) : base(dbc, logFac, svp)
        {
        }
        protected override void LoginFail(ActionExecutingContext context)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public JsonReturn Register([FromForm]string username, [FromForm]string password)
        {
            username = XSSConvert(username);
            string salt = HashStr(username + DateTime.Now.ToString());
            string passHash = HashStr(salt + password + salt + username);
            string ip = new HttpParser(HttpContext).GetIPAddr();
            var loginIPDic = new Dictionary<string, bool>();
            loginIPDic.Add(ip, true);
            UserEntity u = new UserEntity { Name = username, Pass = passHash, Salt = salt, LoginIP = loginIPDic };
            try
            {
                dbc.User.Add(u);
                dbc.SaveChanges();
            }
            catch (Exception e) { return JsonReturn.ReturnFail("Username already exists!"); }
            return JsonReturn.ReturnSuccess();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public JsonReturn Login([FromForm]string username, [FromForm]string password)
        {
            username = XSSConvert(username);
            var domain = new HttpParser(HttpContext).GetDomain();
            UserEntity u = (from lu in dbc.User where lu.Name == username select lu).FirstOrDefault();
            if (u == null) { return JsonReturn.ReturnFail(-1, "Wrong username or password！"); }
            string salt = u.Salt;
            string passHash = HashStr(salt + password + salt + username);
            if (u.Pass != passHash) { return JsonReturn.ReturnFail(-1, "Wrong username or password！"); }
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
