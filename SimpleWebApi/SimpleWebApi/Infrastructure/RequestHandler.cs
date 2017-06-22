using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure
{
    public interface IRequest { }

    public interface IRequest<TResponse> { }

    public interface IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        Task Handle(TRequest request);
    }

    public interface IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }
}