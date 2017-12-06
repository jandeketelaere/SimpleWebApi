using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

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
        public async Task<ApiResult> Handle(IRequest request, IServiceProvider services)
        {
            var handler = GetRequestHandlerDelegate(request, services);

            foreach (var decorator in services.GetServices(typeof(IRequestHandlerDecorator<TRequest, Unit>)).Cast<IRequestHandlerDecorator<TRequest, Unit>>())
            {
                var previousHandler = handler;
                handler = () => decorator.Handle((TRequest)request, previousHandler);
            }

            return ApiResult.Translate(await handler());
        }

        private static RequestHandlerDelegate<Unit> GetRequestHandlerDelegate(IRequest request, IServiceProvider services)
        {
            if (HandlerExists<IAsyncRequestHandler<TRequest>>(services))
            {
                return async () =>
                {
                    var handler = GetHandler<IAsyncRequestHandler<TRequest>>(services);
                    var response = await handler.Handle((TRequest)request);
                    return ApiResult<Unit>.Translate(response, Unit.Value);
                };
            }

            if (HandlerExists<IRequestHandler<TRequest>>(services))
            {
                return async () =>
                {
                    var handler = GetHandler<IRequestHandler<TRequest>>(services);
                    var response = await Task.FromResult(handler.Handle((TRequest)request));
                    return ApiResult<Unit>.Translate(response, Unit.Value);
                };
            }

            throw new InvalidOperationException($"No handler was found for request of type {request.GetType()}");
        }

        private static bool HandlerExists<THandler>(IServiceProvider services) => GetHandler<THandler>(services) != null;

        private static THandler GetHandler<THandler>(IServiceProvider services) => (THandler)services.GetService(typeof(THandler));
    }

    public class RequestHandlerWrapper<TRequest, TResponse> : IRequestHandlerWrapper<TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<ApiResult<TResponse>> Handle(IRequest<TResponse> request, IServiceProvider services)
        {
            var handler = GetRequestHandlerDelegate(request, services);

            foreach (var decorator in services.GetServices(typeof(IRequestHandlerDecorator<TRequest, TResponse>)).Cast<IRequestHandlerDecorator<TRequest, TResponse>>())
            {
                var previousHandler = handler;
                handler = () => decorator.Handle((TRequest)request, previousHandler);
            }

            return await handler();
        }

        private static RequestHandlerDelegate<TResponse> GetRequestHandlerDelegate(IRequest<TResponse> request, IServiceProvider services)
        {
            if (HandlerExists<IAsyncRequestHandler<TRequest, TResponse>>(services))
            {
                return async () =>
                {
                    var handler = GetHandler<IAsyncRequestHandler<TRequest, TResponse>>(services);
                    return await handler.Handle((TRequest)request);
                };
            }

            if (HandlerExists<IRequestHandler<TRequest, TResponse>>(services))
            {
                return async () =>
                {
                    var handler = GetHandler<IRequestHandler<TRequest, TResponse>>(services);
                    return await Task.FromResult(handler.Handle((TRequest)request));
                };
            }

            throw new InvalidOperationException($"No handler was found for request of type {request.GetType()}");
        }

        private static bool HandlerExists<THandler>(IServiceProvider services) => GetHandler<THandler>(services) != null;

        private static THandler GetHandler<THandler>(IServiceProvider services) => (THandler)services.GetService(typeof(THandler));
    }
}