using System;
using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure
{
    public interface IRequestHandlerWrapper
    {
        Task<ApiResult> Handle(IRequest request, IServiceProvider services);
    }

    public interface IRequestHandlerWrapper<TResponse>
    {
        Task<ApiResult<TResponse>> Handle(IRequest<TResponse> request, IServiceProvider services);
    }

    public class RequestHandlerWrapper<TRequest> : IRequestHandlerWrapper where TRequest : IRequest
    {
        public Task<ApiResult> Handle(IRequest request, IServiceProvider services)
        {
            var asyncHandler = GetHandler<IAsyncRequestHandler<TRequest>>(services);

            if (asyncHandler != null)
                return asyncHandler.Handle((TRequest)request);

            var handler = GetHandler<IRequestHandler<TRequest>>(services);

            if (handler != null)
                return Task.FromResult(handler.Handle((TRequest)request));

            throw new InvalidOperationException($"No handler was found for request of type {request.GetType()}");
        }

        private static THandler GetHandler<THandler>(IServiceProvider services)
        {
            return (THandler)services.GetService(typeof(THandler));
        }
    }

    public class RequestHandlerWrapper<TRequest, TResponse> : IRequestHandlerWrapper<TResponse> where TRequest : IRequest<TResponse>
    {
        public Task<ApiResult<TResponse>> Handle(IRequest<TResponse> request, IServiceProvider services)
        {
            var asyncHandler = GetHandler<IAsyncRequestHandler<TRequest, TResponse>>(services);

            if (asyncHandler != null)
                return asyncHandler.Handle((TRequest)request);

            var handler = GetHandler<IRequestHandler<TRequest, TResponse>>(services);

            if (handler != null)
                return Task.FromResult(handler.Handle((TRequest)request));

            throw new InvalidOperationException($"No handler was found for request of type {request.GetType()}");
        }

        private static THandler GetHandler<THandler>(IServiceProvider services)
        {
            return (THandler) services.GetService(typeof(THandler));
        }
    }
}