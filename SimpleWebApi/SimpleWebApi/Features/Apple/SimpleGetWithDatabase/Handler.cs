using SimpleWebApi.Entities;
using SimpleWebApi.Infrastructure;
using System.Threading.Tasks;

namespace SimpleWebApi.Features.Apple.SimpleGetWithDatabase
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly SimpleWebApiContext _context;

        public Handler(SimpleWebApiContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var apple = new Entities.Apple
            {
                Name = request.Name
            };

            await _context.AddAsync(apple);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response());
        }
    }
}