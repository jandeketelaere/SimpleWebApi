using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleWebApi.Infrastructure;

namespace SimpleWebApi.Features.Apple
{
    public class Get
    {
        public class Request : IRequest<Response>
        {
            public int Id { get; set; }
        }

        public class Response
        {
            public IEnumerable<string> Apples { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public async Task<Response> Handle(Request request)
            {
                await Task.Delay(1000);

                return new Response
                {
                    Apples = new[] { "apple1", "apple2" }
                };
            }
        }
    }
}