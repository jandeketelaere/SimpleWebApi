using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure
{
    public interface IRequest<TResponse> { }

    public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<ApiResult<TResponse>> Handle(TRequest request);
    }
}