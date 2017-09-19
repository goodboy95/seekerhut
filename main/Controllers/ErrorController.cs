using Microsoft.AspNetCore.Mvc;
using Dao;
using Utils;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;

namespace web.Controllers
{
    public class ErrorController : ViewBaseController
    {
        public ErrorController(DwDbContext dbc, ILoggerFactory logFac) : base(dbc, logFac)
        {
        }
        protected override void LoginFail(ActionExecutingContext context)
        {
        }
        public IActionResult Page404() => View();
        public IActionResult Page500() => View();
        public IActionResult Api404() => JsonReturn.ReturnFail(404, "该接口不存在！");
        public IActionResult Api500() => JsonReturn.ReturnFail(500, "该接口出现内部错误，无法访问！");

    }
}
