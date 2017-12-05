using SimpleWebApi.Infrastructure;

namespace SimpleWebApi.Features.Apple.SimpleGetWithValidation
{
    public class Handler : IRequestHandler<Request, Response>
    {
        public ApiResult<Response> Handle(Request request)
        {
            var response = new Response
            {
                Name = "Mr Apple"
            };

            return ApiResult<Response>.Ok(response);
        }
    }
}