using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace FlowTimer.Infrastructure.Extensions
{
    public static class LoggerExtensions
    {
        public static void AddLogger(this ILoggingBuilder logging, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            logging.ClearProviders();
            logging.AddSerilog(logger);
        }
    }
}