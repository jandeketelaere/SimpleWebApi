using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using SimpleWebApi.Features.Apple;
using SimpleWebApi.Infrastructure;
using System.Reflection;

namespace SimpleWebApi
{
    public class Startup
    {
        private Container container = new Container();

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));
            services.UseSimpleInjectorAspNetRequestScoping(container);

            RegisterTypes();
        }

        private void RegisterTypes()
        {
            container.Register(typeof(IAsyncRequestHandler<,>), new[] { typeof(IAsyncRequestHandler<,>).GetTypeInfo().Assembly }, Lifestyle.Singleton);
            container.Register(typeof(IAsyncRequestHandler<>), new[] { typeof(IAsyncRequestHandler<>).GetTypeInfo().Assembly }, Lifestyle.Singleton);

            container.Register(typeof(IAsyncRequestValidator<>), new[] { typeof(IAsyncRequestValidator<>).GetTypeInfo().Assembly }, Lifestyle.Singleton);
            container.RegisterConditional(typeof(IAsyncRequestValidator<>), typeof(NullAsyncRequestValidator<>), Lifestyle.Singleton, c => !c.Handled);

            container.RegisterDecorator(typeof(IAsyncRequestHandler<,>), typeof(AsyncRequestHandlerValidationDecorator<,>), Lifestyle.Singleton);
            container.RegisterDecorator(typeof(IAsyncRequestHandler<>), typeof(AsyncRequestHandlerValidationDecorator<>), Lifestyle.Singleton);

            container.RegisterSingleton<IAsyncRequestHandlerMediator>(new AsyncRequestHandlerMediator(container));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}