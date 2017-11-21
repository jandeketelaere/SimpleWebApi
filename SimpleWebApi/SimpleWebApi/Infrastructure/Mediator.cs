using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure
{
    public interface IMediator
    {
        Task<IActionResult> Send<TResponse>(IRequest<TResponse> request);
    }

    public class Mediator : IMediator
    {
        private readonly IServiceProvider _services;

        public Mediator(IServiceProvider services)
        {
            _services = services;
        }

        public async Task<IActionResult> Send<TResponse>(IRequest<TResponse> request)
        {
            dynamic handler = GetHandler(request.GetType(), typeof(TResponse));

            var result = await handler.Handle((dynamic)request);

            return new ObjectResult(result.IsSuccessful ? result.Value : new Error { ErrorMessage = result.ErrorMessage })
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }

        public object GetHandler(Type requestType, Type responseType)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

            return _services.GetService(handlerType);
        }
    }
}