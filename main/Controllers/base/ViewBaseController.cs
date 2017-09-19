using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dao;
using Domain.Entity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;

namespace web.Controllers
{
    public abstract class ViewBaseController : BaseController
    {
        public ViewBaseController(DwDbContext dbc, ILoggerFactory logFac) : base(dbc, logFac)
        {
        }
        protected override void LoginFail(ActionExecutingContext context)
        {
            context.Result = Redirect("/home/logout/");
        }
    }
}