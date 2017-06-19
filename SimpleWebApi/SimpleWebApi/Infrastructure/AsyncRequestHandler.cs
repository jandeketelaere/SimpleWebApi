using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure
{
    public interface IRequest { }

    public interface IRequest<TResponse> { }

    public interface IAsyncRequestHandler<TRequest>
        where TRequest : IRequest
    {
        Task HandleAsync(TRequest request);
    }

    public interface IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request);
    }
}