using SimpleWebApi.Infrastructure;

namespace SimpleWebApi.Features.Apple.SimpleGetWithMultipleValidations
{
    public class Request : IRequest<Response>
    {
        public string Name { get; set; }
    }
}