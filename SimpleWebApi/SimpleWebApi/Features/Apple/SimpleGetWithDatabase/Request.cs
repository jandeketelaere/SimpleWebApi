using SimpleWebApi.Infrastructure;

namespace SimpleWebApi.Features.Apple.SimpleGetWithDatabase
{
    public class Request : IRequest
    {
        public string Name { get; set; }
    }
}