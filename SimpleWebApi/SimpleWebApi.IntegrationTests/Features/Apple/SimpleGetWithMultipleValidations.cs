using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Xunit;
using static SimpleWebApi.IntegrationTests.TestHelper;
using Request = SimpleWebApi.Features.Apple.SimpleGetWithMultipleValidations.Request;

namespace SimpleWebApi.IntegrationTests.Features.Apple
{
    public class SimpleGetWithMultipleValidations
    {
        [Fact]
        public async Task ShouldFailWhenNameIsEmpty()
        {
            var request = new Request
            {
                Name = string.Empty
            };

            var response = await Send(request);

            response.IsFailure.ShouldBeTrue();
            response.HttpStatusCode.ShouldBe(HttpStatusCode.BadRequest);
            response.ErrorMessage.ShouldBe("Name is required");
        }

        [Fact]
        public async Task ShouldFailWhenAppleAlreadyExists()
        {
            const string name = "ExistingApple";

            var existingApple = new Entities.Apple
            {
                Name = name
            };

            await ExecuteDbContext(db => db.Apples.AddAsync(existingApple));

            var request = new Request
            {
                Name = name
            };

            var response = await Send(request);

            response.IsFailure.ShouldBeTrue();
            response.HttpStatusCode.ShouldBe(HttpStatusCode.BadRequest);
            response.ErrorMessage.ShouldBe("An apple with name ExistingApple already exists");
        }
    }
}