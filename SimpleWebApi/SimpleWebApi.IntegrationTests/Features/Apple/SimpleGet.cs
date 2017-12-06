using System.Threading.Tasks;
using Shouldly;
using Xunit;
using static SimpleWebApi.IntegrationTests.TestHelper;
using Request = SimpleWebApi.Features.Apple.SimpleGet.Request;

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

            var response = await Mediator.Send(request);

            response.IsSuccessful.ShouldBeTrue();
            response.Value.Name.ShouldBe("Mr Apple");
        }
    }
}