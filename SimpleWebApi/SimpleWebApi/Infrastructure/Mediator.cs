using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure
{
    public interface IMediator
    {
        Task<IActionResult> Send(IRequest request);
        Task<IActionResult> Send<TResponse>(IRequest<TResponse> request);
    }

    public class Mediator : IMediator
    {
        private readonly IServiceProvider _services;

        public Mediator(IServiceProvider services)
        {
            _services = services;
        }

        public async Task<IActionResult> Send(IRequest request)
        {
            var handlerType = typeof(RequestHandlerWrapper<>).MakeGenericType(request.GetType());
            var handlerWrapper = (IRequestHandlerWrapper)Activator.CreateInstance(handlerType);

            var result = await handlerWrapper.Handle(request, _services);

            return result.IsSuccessful
                ? new ObjectResult(null)
                    { StatusCode = (int)result.HttpStatusCode }

                : new ObjectResult(new Error { ErrorMessage = result.ErrorMessage })
                    { StatusCode = (int)result.HttpStatusCode };
        }

        public async Task<IActionResult> Send<TResponse>(IRequest<TResponse> request)
        {
            var handlerType = typeof(RequestHandlerWrapper<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handlerWrapper = (IRequestHandlerWrapper<TResponse>)Activator.CreateInstance(handlerType);

            var result = await handlerWrapper.Handle(request, _services);

            return result.IsSuccessful
                ? new ObjectResult(result.Value)
                { StatusCode = (int)result.HttpStatusCode }

                : new ObjectResult(new Error { ErrorMessage = result.ErrorMessage })
                { StatusCode = (int)result.HttpStatusCode };
        }
    }
}