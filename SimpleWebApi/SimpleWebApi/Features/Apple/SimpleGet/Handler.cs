using System.Threading.Tasks;
using SimpleWebApi.Infrastructure;
using SimpleWebApi.Features.Apple.SimpleGet;

namespace SimpleWebApi.Features.Apple
{
    public class Handler : IRequestHandler<Request, Response>
    {
        public async Task<ApiResult<Response>> Handle(Request request)
        {
            await Task.Delay(10);

            var response = new Response
            {
                Name = "Mr Apple"
            };

            return ApiResult<Response>.Ok(response);
        }
    }
}