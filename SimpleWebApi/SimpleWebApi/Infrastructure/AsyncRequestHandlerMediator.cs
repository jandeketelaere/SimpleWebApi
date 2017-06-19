using System;
using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure
{
    public interface IAsyncRequestHandlerMediator
    {
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request);
        Task SendAsync(IRequest request);
    }

    public class AsyncRequestHandlerMediator : IAsyncRequestHandlerMediator
    {
        private readonly IServiceProvider _services;

        public AsyncRequestHandlerMediator(IServiceProvider services)
        {
            _services = services;
        }

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            var handlerType = typeof(IAsyncRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            
            dynamic handler = _services.GetService(handlerType);

            return handler.HandleAsync((dynamic)request);
        }

        public Task SendAsync(IRequest request)
        {
            var handlerType = typeof(IAsyncRequestHandler<>).MakeGenericType(request.GetType());

            dynamic handler = _services.GetService(handlerType);

            return handler.HandleAsync((dynamic)request);
        }
    }
}