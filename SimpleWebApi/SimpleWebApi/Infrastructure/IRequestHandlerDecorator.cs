using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure
{
    public delegate Task<ApiResult<TResponse>> RequestHandlerDelegate<TResponse>();

    public interface IRequestHandlerDecorator<in TRequest, TResponse>
    {
        Task<ApiResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next);
    }
}