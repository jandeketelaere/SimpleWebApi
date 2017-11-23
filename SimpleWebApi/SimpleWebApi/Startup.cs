using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using SimpleWebApi.Infrastructure;
using SimpleWebApi.Infrastructure.Decorators;
using SimpleWebApi.Infrastructure.Logging;
using SimpleWebApi.Infrastructure.Middleware;

namespace SimpleWebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .CreateLogger();
        }

        public IConfigurationRoot Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<IMediator, Mediator>();
            services.AddScoped<IApiLogger, ApiLogger>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            ConfigureHandlers(services);
            ConfigureDecorators(services);
            ConfigureValidators(services);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            app.UseCustomExceptionHandler();
            app.UseRequestResponseLogging();

            app.UseMvc();
        }

        private static void ConfigureHandlers(IServiceCollection services)
        {
            services.Scan(scan => scan.FromEntryAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );
        }

        private static void ConfigureDecorators(IServiceCollection services)
        {
            services.Decorate(typeof(IRequestHandler<,>), typeof(ValidationDecorator<,>));
        }

        private static void ConfigureValidators(IServiceCollection services)
        {
            services.Scan(scan => scan.FromEntryAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );
        }
    }
}