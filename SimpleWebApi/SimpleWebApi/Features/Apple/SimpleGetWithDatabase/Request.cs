using SimpleWebApi.Infrastructure;

namespace SimpleWebApi.Features.Apple.SimpleGetWithDatabase
{
    public class Request : IRequest<Response>
    {
        public string Name { get; set; }
    }
}