using SimpleWebApi.Infrastructure;

namespace SimpleWebApi.Features.Apple.SimpleGet
{
    public class Request : IRequest<Response>
    {
        public int Id { get; set; }
    }
}