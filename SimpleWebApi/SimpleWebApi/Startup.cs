using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using SimpleWebApi.Features.Apple;
using SimpleWebApi.Infrastructure;
using SimpleWebApi.Infrastructure.Decorators;
using SimpleWebApi.Infrastructure.Middleware;
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
            services
                .AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));
            services.UseSimpleInjectorAspNetRequestScoping(container);

            RegisterTypes();
        }

        private void RegisterTypes()
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.Register(typeof(IRequestHandler<,>), new[] { typeof(IRequestHandler<,>).GetTypeInfo().Assembly });
            container.Register(typeof(IRequestHandler<>), new[] { typeof(IRequestHandler<>).GetTypeInfo().Assembly });

            container.Register(typeof(IValidator<>), new[] { typeof(Get.Validator) });
            container.RegisterConditional(typeof(IValidator<>), typeof(NullValidator<>), c => !c.Handled);

            container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(RequestHandlerValidator<,>));
            container.RegisterDecorator(typeof(IRequestHandler<>), typeof(RequestHandlerValidator<>));

            container.RegisterSingleton<IMediator>(new Mediator(container));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCustomExceptionHandler();

            app.UseMvc();
        }
    }
}