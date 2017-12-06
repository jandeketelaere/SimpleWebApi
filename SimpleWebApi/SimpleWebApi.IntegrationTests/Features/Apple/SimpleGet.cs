﻿using System.Threading.Tasks;
using Shouldly;
using Xunit;
using static SimpleWebApi.IntegrationTests.TestHelper;
using Request = SimpleWebApi.Features.Apple.SimpleGet.Request;
using System.Net;
using SimpleWebApi.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleWebApi.IntegrationTests.Features.Apple
{
    public class SimpleGet
    {
        [Fact]
        public async Task ShouldReturnMrApple()
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetService<IMediator>();

                var request = new Request
                {
                    Id = 1
                };

                var response = await mediator.Send(request);

                response.IsSuccessful.ShouldBeTrue();
                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);
                response.Value.Name.ShouldBe("Mr Apple");
            }
        }
    }
}