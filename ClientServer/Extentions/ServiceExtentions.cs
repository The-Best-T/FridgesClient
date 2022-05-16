using Contracts;
using MessengerService;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace ClientServer.Extentions
{
    public static class ServiceExtentions
    {
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddScoped<IMessenger, MessengerManager>();
            services.AddScoped<ILoggerManager, LoggerManager>();
        }

    }
}
