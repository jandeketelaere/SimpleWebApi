using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static SimpleWebApi.IntegrationTests.TestHelper;
using Request = SimpleWebApi.Features.Apple.SimpleGetWithDatabase.Request;
using Microsoft.Extensions.DependencyInjection;
using SimpleWebApi.Infrastructure;
using SimpleWebApi.Entities;

namespace SimpleWebApi.IntegrationTests.Features.Apple
{
    public class SimpleGetWithDatabase
    {
        [Fact]
        public async Task ShouldCreateAnApple()
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetService<IMediator>();
                var context = scope.ServiceProvider.GetService<SimpleWebApiContext>();

                var name = "Apple";

                var request = new Request
                {
                    Name = name
                };

                var response = await mediator.Send(request);

                response.IsSuccessful.ShouldBeTrue();
                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                var newApple = await context.Apples.SingleOrDefaultAsync(a => a.Id == response.Value.Id);

                newApple.ShouldNotBeNull();
                newApple.Name.ShouldBe(name);
            }
        }
    }
}