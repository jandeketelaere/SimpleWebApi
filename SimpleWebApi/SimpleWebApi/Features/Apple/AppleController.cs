using Microsoft.AspNetCore.Mvc;
using SimpleWebApi.Features.Apple;
using SimpleWebApi.Infrastructure;
using System.Threading.Tasks;

namespace SimpleWebApi.Features.Apple
{
    [Route("api/apples")]
    public class AppleController : Controller
    {
        private readonly IMediator _mediator;

        public AppleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //api/apples/1
        [HttpGet("{id}")]
        public async Task<Get.Response> Get(Get.Request query)
        {
            return await _mediator.Send(query);
        }
    }
}