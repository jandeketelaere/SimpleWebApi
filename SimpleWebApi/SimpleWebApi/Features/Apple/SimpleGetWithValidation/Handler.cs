using SimpleWebApi.Infrastructure;
using System.Threading.Tasks;

namespace SimpleWebApi.Features.Apple.SimpleGetWithValidation
{
    public class Handler : IRequestHandler<Request, Response>
    {
        public async Task<ApiResult<Response>> Handle(Request request)
        {
            await Task.Delay(10);

            if (request.Id == 0)
                return ApiResult<Response>.BadRequest("Id cannot be 0");

            var response = new Response
            {
                Name = "Mr Apple"
            };

            return ApiResult<Response>.Ok(response);
        }
    }
}