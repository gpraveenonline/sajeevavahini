using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NLog;

namespace SV.Infrastructure.Filters
{
    public class GlobalExceptionLoggerFilter : IExceptionFilter
    {
        Microsoft.Extensions.Logging.ILogger logger;

        NLog.ILogger nlogger;

        public GlobalExceptionLoggerFilter(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<GlobalExceptionLoggerFilter>();
        }

        public GlobalExceptionLoggerFilter(ILoggingBuilder loggingBuilder)
        {
            nlogger = loggingBuilder as Logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (logger != null)
                logger.LogError(context.Exception.Message);
            else
                nlogger.LogException(NLog.LogLevel.Error, context.Exception.Message, context.Exception);
        }
    }
}
