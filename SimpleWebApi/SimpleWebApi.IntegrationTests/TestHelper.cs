using System.IO;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;
using SimpleWebApi.Entities;
using SimpleWebApi.Infrastructure;

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

        private static async Task ExecuteScopeWithTransaction(Func<IServiceProvider, Task> action)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<SimpleWebApiContext>();

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await action(scope.ServiceProvider);

                        await context.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private static async Task<T> ExecuteScopeWithTransaction<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<SimpleWebApiContext>();

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var result = await action(scope.ServiceProvider);

                        await context.SaveChangesAsync();
                        transaction.Commit();

                        return result;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private static async Task<T> ExecuteScopeWithoutTransaction<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var result = await action(scope.ServiceProvider);

                return result;
            }
        }

        public static Task<ApiResult<TResponse>> Send<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeWithoutTransaction(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public static Task<ApiResult> Send(IRequest request)
        {
            return ExecuteScopeWithoutTransaction(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public static Task ExecuteDbContext(Func<SimpleWebApiContext, Task> action)
        {
            return ExecuteScopeWithTransaction(sp => action(sp.GetService<SimpleWebApiContext>()));
        }

        public static Task<T> ExecuteDbContext<T>(Func<SimpleWebApiContext, Task<T>> action)
        {
            return ExecuteScopeWithTransaction(sp => action(sp.GetService<SimpleWebApiContext>()));
        }
    }
}