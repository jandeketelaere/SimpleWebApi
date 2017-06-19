using Microsoft.AspNetCore.Mvc;
using SimpleWebApi.Infrastructure;
using System.Threading.Tasks;

namespace SimpleWebApi.Features.Apple
{
    
    public class AppleController : Controller
    {
        private readonly IAsyncRequestHandlerMediator _mediator;

        public AppleController(IAsyncRequestHandlerMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("api/apples/get/{id}")]
        public async Task<Get.Response> Get(Get.Request request)
        {
            return await _mediator.SendAsync(request);
        }

        [HttpGet]
        [Route("api/apples/delete/{id}")]
        public async Task Delete(Delete.Request query)
        {
            await _mediator.SendAsync(query);
        }
    }
}