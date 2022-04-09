using Contracts;
using MessengerService;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace ClientServer.Extentions
{
    public static class ServiceExtentions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });
        }
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<IMessenger, MessengerManager>();
            services.AddScoped<ILoggerManager, LoggerManager>();
        }

    }
}
