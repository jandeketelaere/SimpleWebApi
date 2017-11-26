using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using SimpleWebApi.Entities;
using SimpleWebApi.Infrastructure;
using SimpleWebApi.Infrastructure.Logging;
using SimpleWebApi.Infrastructure.Middleware;
using SimpleWebApi.Infrastructure.Transaction;
using SimpleWebApi.Infrastructure.Validation;
using FluentValidation.AspNetCore;

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
            services
                .AddMvc()
                .AddFluentValidation();

            Configure(services);
            ConfigureEntityFramework(services);
            ConfigureHandlers(services);
            ConfigureDecorators(services);
            ConfigureValidators(services);
        }

        private static void ConfigureEntityFramework(IServiceCollection services)
        {
            services.AddDbContext<SimpleWebApiContext>(options => options.UseSqlServer("Data Source=localhost;Initial Catalog=SimpleWebApi;Integrated Security=SSPI;"));
        }

        private static void Configure(IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();
            services.AddScoped<IApiLogger, ApiLogger>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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

        //Order of registration is important. The decorator that gets registered last will be the first to execute
        private static void ConfigureDecorators(IServiceCollection services)
        {
            services.Decorate(typeof(IRequestHandler<,>), typeof(TransactionDecorator<,>));
            services.Decorate(typeof(IRequestHandler<,>), typeof(ValidationDecorator<,>));
        }

        private static void ConfigureValidators(IServiceCollection services)
        {
            services.Scan(scan => scan.FromEntryAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IApiValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );
        }
    }
}