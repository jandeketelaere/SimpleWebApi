using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Xunit;
using static SimpleWebApi.IntegrationTests.TestHelper;
using Request = SimpleWebApi.Features.Apple.SimpleGetWithValidation.Request;

namespace SimpleWebApi.IntegrationTests.Features.Apple
{
    public class SimpleGetWithValidation
    {
        [Fact]
        public async Task ShouldFailWhenIdIsZero()
        {
            var request = new Request
            {
                Id = 0
            };

            var response = await Send(request);

            response.IsFailure.ShouldBeTrue();
            response.HttpStatusCode.ShouldBe(HttpStatusCode.BadRequest);
            response.ErrorMessage.ShouldBe("Id cannot be 0");
        }
    }
}