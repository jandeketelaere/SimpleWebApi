using System.IO;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;

namespace SimpleWebApi.IntegrationTests
{
    public static class TestHelper
    {
        public static readonly IServiceScopeFactory ScopeFactory;

        static TestHelper()
        {
            var host = A.Fake<IHostingEnvironment>();

            A.CallTo(() => host.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            var startup = new Startup(host);
            var serviceCollection = new ServiceCollection();
            startup.ConfigureServices(serviceCollection);
            var provider = serviceCollection.BuildServiceProvider();
            ScopeFactory = provider.GetService<IServiceScopeFactory>();
        }

        public static async Task RunTest(Task<Action<IServiceScope>> action)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                (await action)(scope);
            }
        }
    }
}