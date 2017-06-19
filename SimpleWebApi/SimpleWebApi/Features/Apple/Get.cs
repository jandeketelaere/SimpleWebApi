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

        public class Validator : IAsyncRequestValidator<Request>
        {
            public async Task<ValidationResult> ValidateAsync(Request request)
            {
                return new ValidationResult
                {
                    IsSuccessful = false
                };
            }
        }

        public class Handler : IAsyncRequestHandler<Request, Response>
        {
            public async Task<Response> HandleAsync(Request request)
            {
                return new Response
                {
                    Apples = new[] { "apple1", "apple2" }
                };
            }
        }
    }
}