using System;
using System.IO;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SimpleWebApi.Infrastructure;

namespace SimpleWebApi.IntegrationTests
{
    public static class TestHelper
    {
        private static readonly IServiceProvider Services;

        static TestHelper()
        {
            var host = A.Fake<IHostingEnvironment>();

            A.CallTo(() => host.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            var startup = new Startup(host);
            var serviceCollection = new ServiceCollection();
            startup.ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();
        }

        public static IMediator Mediator => Services.GetService<IMediator>();
    }
}