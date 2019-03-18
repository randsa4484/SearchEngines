using System;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Bds.TechTest.Filters
{
    public class ExceptionFilterAttribute : ActionFilterAttribute, IExceptionFilter
    {
        private readonly ILogger<ExceptionFilterAttribute> _logger;

        public ExceptionFilterAttribute(ILogger<ExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.HttpContext?.Response == null)
                return;
            
            if (context.Exception is NotImplementedException)
            {
                context.HttpContext.Response.Clear();
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotImplemented;
                context.HttpContext.Response.WriteAsync("The action requested is not implemented", new CancellationToken()).Wait();
            }
            else
            {
                context.HttpContext.Response.Clear();
                context.HttpContext.Response.StatusCode = 500;
                context.HttpContext.Response.WriteAsync("An exception has been thrown. See logs for details", new CancellationToken()).Wait();
                _logger.LogError(context.Exception, "");
            }

            context.ExceptionHandled = true;
        }
    }
}
