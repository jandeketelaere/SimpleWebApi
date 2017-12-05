using SimpleWebApi.Entities;
using SimpleWebApi.Infrastructure;
using System.Threading.Tasks;

namespace SimpleWebApi.Features.Apple.SimpleGetWithDatabase
{
    public class Handler : IAsyncRequestHandler<Request>
    {
        private readonly SimpleWebApiContext _context;

        public Handler(SimpleWebApiContext context)
        {
            _context = context;
        }

        public async Task<ApiResult> Handle(Request request)
        {
            var apple = new Entities.Apple
            {
                Name = request.Name
            };

            await _context.AddAsync(apple);

            return ApiResult.Ok();
        }
    }
}