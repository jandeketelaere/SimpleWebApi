using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static SimpleWebApi.IntegrationTests.TestHelper;
using Request = SimpleWebApi.Features.Apple.SimpleGetWithDatabase.Request;

namespace SimpleWebApi.IntegrationTests.Features.Apple
{
    public class SimpleGetWithDatabase
    {
        [Fact]
        public async Task ShouldCreateAnApple()
        {
            const string name = "NewApple";

            var request = new Request
            {
                Name = name
            };

            var response = await Send(request);

            response.IsSuccessful.ShouldBeTrue();
            response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);
            
            var newApple = await ExecuteDbContext(db => db.Apples.SingleOrDefaultAsync(a => a.Id == response.Value.Id));

            newApple.ShouldNotBeNull();
            newApple.Name.ShouldBe(name);
        }
    }
}