using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Utils
{
    public class HttpParser
    {
        protected HttpContext _context;
        public HttpParser(HttpContext hc)
        {
            _context = hc;
        }
        public HttpParser(ActionExecutingContext aec)
        {
            _context = aec.HttpContext;
        }
        public HttpParser(ActionExecutedContext aedc)
        {
            _context = aedc.HttpContext;
        }

        public string GetDomain(int parts = 2, bool prefixDot = true)
        {
            var request = _context.Request;
            var hostUrl = request.Headers["Host"][0];
            var fullDomainName = hostUrl.Split(':')[0];
            var domainLength = fullDomainName.Length;
            int i;
            //判断是否用ip直接登录，ipv4 and ipv6
            if (Regex.IsMatch(fullDomainName, "^(\\d+\\.){3}\\d+$")) { return fullDomainName; }
            else if (Regex.IsMatch(fullDomainName, "^\\[\\d*$")) { return Regex.Match(hostUrl, "\\[.+\\]").Value; }
            //判断是否用类似于localhost的主机名（即不带点的“域名”）登录
            else if (fullDomainName.IndexOf(".") == -1) { return fullDomainName; }
            for (i = domainLength - 1; i >= 0; i--)
            {
                char c = fullDomainName[i];
                if (c == '.') { parts--; }
                if (parts <= 0) { break; }
            }
            if (prefixDot == true && i <= 0) { return "." + fullDomainName; }
            else if (prefixDot == false && i != domainLength - 1) { i++; }
            return fullDomainName.Substring(i);
        }
        public string GetIPAddr()
        {
            var ip = _context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = _context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
        
    }
}