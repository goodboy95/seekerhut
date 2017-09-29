using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;

namespace Middleware
{
    public class ErrorPageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly StatusCodePagesOptions _options;
        private static readonly IDictionary<int, string> _errorPages = new Dictionary<int, string>();

        public ErrorPageMiddleware(RequestDelegate next, ILoggerFactory logFac, IOptions<StatusCodePagesOptions> options)
        {
            _next = next;
            _logger = logFac.CreateLogger<ErrorPageMiddleware>();
            _options = options.Value;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "An unhandled exception has occurred while executing the request");
            }
            finally
            {
                if (!context.Response.HasStarted)
                {
                    var statusCode = context.Response.StatusCode;
                    var requestPath = context.Request.Path.ToString();
                    if (requestPath.Contains("api/"))
                    {
                        switch(statusCode)
                        {
                            case 404: context.Response.Redirect("/error/api404"); break;
                            case 500: context.Response.Redirect("/error/api500"); break;
                            default: break;
                        }
                    }
                    else
                    {
                        switch(statusCode)
                        {
                            case 404: context.Response.Redirect("/error/page404"); break;
                            case 500: context.Response.Redirect("/error/page500"); break;
                            
                            default: break;
                        }
                    }
                }
                await _next.Invoke(context);
            /*  if (statusCode == 404)
                {
                    if (requestController == "api")
                    {
                        context.Request.Path = "/error/api404";
                        await _next.Invoke(context);
                    }
                    else
                    {
                        context.Request.Path = "/error/page404";
                        await _next.Invoke(context);
                    }
                }
                else if (statusCode == 500)
                {
                    if (requestController == "api")
                    {
                        context.Request.Path = "/error/api500";
                        await _next.Invoke(context);
                    }
                    else
                    {
                        context.Request.Path = "/error/page500";
                        await _next.Invoke(context);
                    }
                }*/
            }
        }
    }
}


               