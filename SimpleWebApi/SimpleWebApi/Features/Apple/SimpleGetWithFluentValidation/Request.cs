using SimpleWebApi.Infrastructure;

namespace SimpleWebApi.Features.Apple.SimpleGetWithFluentValidation
{
    public class Request : IRequest<Response>
    {
        public int Id { get; set; }
    }
}