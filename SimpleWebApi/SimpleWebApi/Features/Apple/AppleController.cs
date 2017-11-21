using Microsoft.AspNetCore.Mvc;
using SimpleWebApi.Infrastructure;
using System.Threading.Tasks;

namespace SimpleWebApi.Features.Apple
{
    public class AppleController : Controller
    {
        private readonly IMediator _mediator;

        public AppleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("api/apples/simpleget/{id}")]
        public async Task<IActionResult> SimpleGet(SimpleGet.Request request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("api/apples/simplegetwithvalidation/{id}")]
        public async Task<IActionResult> SimpleGetWithValidation(SimpleGetWithValidation.Request request)
        {
            return await _mediator.Send(request);
        }
    }
}