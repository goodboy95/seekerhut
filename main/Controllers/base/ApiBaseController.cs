using System;
using web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dao;
using Utils;
using Domain.Entity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;

namespace web.Api.Controllers
{
    public abstract class ApiBaseController : BaseController
    {
        public ApiBaseController(DwDbContext dbc, ILoggerFactory logFac) : base(dbc, logFac)
        {
        }
        protected override void LoginFail(ActionExecutingContext context)
        {
            context.Result = JsonReturn.ReturnFail(-3, "你没有权限访问此模块！");
        }
    }
}