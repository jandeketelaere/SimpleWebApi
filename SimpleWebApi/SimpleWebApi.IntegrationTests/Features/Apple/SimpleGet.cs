using System.Threading.Tasks;
using Shouldly;
using Xunit;
using static SimpleWebApi.IntegrationTests.TestHelper;
using Request = SimpleWebApi.Features.Apple.SimpleGet.Request;
using System.Net;

namespace SimpleWebApi.IntegrationTests.Features.Apple
{
    public class SimpleGet
    {
        [Fact]
        public async Task ShouldReturnMrApple()
        {
            var request = new Request
            {
                Id = 1
            };

            var response = await Send(request);

            response.IsSuccessful.ShouldBeTrue();
            response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);
            response.Value.Name.ShouldBe("Mr Apple");
        }
    }
}