using Microsoft.AspNetCore.Mvc;
using SimpleWebApi.Infrastructure;
using System.Threading.Tasks;
using SimpleWebApi.Features.Apple.SimpleGet;
using static SimpleWebApi.Infrastructure.ApiResultToActionResultMapper;

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
            return ToActionResult(await _mediator.Send(request));
        }

        [HttpGet("api/apples/simplegetwithvalidation/{id}")]
        public async Task<IActionResult> SimpleGetWithValidation(SimpleGetWithValidation.Request request)
        {
            return ToActionResult(await _mediator.Send(request));
        }

        [HttpGet("api/apples/simplegetwithmultiplevalidations/{id}")]
        public async Task<IActionResult> SimpleGetWithMultipleValidations(SimpleGetWithMultipleValidations.Request request)
        {
            return ToActionResult(await _mediator.Send(request));
        }

        //Should be HttpPost => easier for debugging :-)
        [HttpGet("api/apples/simplegetwithdatabase/{name}")]
        public async Task<IActionResult> SimpleGetWithDatabase(SimpleGetWithDatabase.Request request)
        {
            return ToActionResult(await _mediator.Send(request));
        }
    }
}