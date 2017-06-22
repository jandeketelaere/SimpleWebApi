using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        [Route("{id}")]
        public async Task<Get.Response> Get(Get.Request request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost]
        [Route("{id}")]
        public async Task Delete(Delete.Request query)
        {
            await _mediator.Send(query);
        }

        [HttpGet]
        public async Task<GetAll.Response> GetAll(GetAll.Request request)
        {
            return await _mediator.Send(request);
        }
    }
}