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

        public class Handler : IRequestHandler<Request>
        {
            public Task Handle(Request request)
            {
                return Task.Delay(10);
            }
        }
    }
}