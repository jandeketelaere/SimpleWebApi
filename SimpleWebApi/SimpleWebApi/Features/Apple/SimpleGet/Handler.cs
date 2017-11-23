using System.Threading.Tasks;
using SimpleWebApi.Infrastructure;

namespace SimpleWebApi.Features.Apple.SimpleGet
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