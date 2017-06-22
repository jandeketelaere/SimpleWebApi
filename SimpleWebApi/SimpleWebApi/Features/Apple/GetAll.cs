using SimpleWebApi.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWebApi.Features.Apple
{
    public class GetAll
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<string> Names { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public async Task<Response> Handle(Request request)
            {
                await Task.Delay(10);

                return new Response
                {
                    Names = new[]
                    {
                        "Mr Apple",
                        "Ms Apple"
                    }
                };
            }
        }
    }
}