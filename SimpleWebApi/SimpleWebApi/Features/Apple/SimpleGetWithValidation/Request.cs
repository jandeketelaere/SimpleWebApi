using SimpleWebApi.Infrastructure;

namespace SimpleWebApi.Features.Apple.SimpleGetWithValidation
{
    public class Request : IRequest<Response>
    {
        public int Id { get; set; }
    }
}