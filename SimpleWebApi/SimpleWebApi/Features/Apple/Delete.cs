using SimpleWebApi.Infrastructure;
using System.Threading.Tasks;

namespace SimpleWebApi.Features.Apple
{
    public class Delete
    {
        public class Request : IRequest
        {
            public int Id { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Request>
        {
            public Task HandleAsync(Request request)
            {
                return Task.CompletedTask;
            }
        }
    }
}