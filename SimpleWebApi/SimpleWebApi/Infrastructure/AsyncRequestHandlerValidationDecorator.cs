using System;
using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure
{
    public class AsyncRequestHandlerValidationDecorator<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> _requestHandler;
        private readonly IAsyncRequestValidator<TRequest> _validator;

        public AsyncRequestHandlerValidationDecorator(IAsyncRequestHandler<TRequest, TResponse> requestHandler, IAsyncRequestValidator<TRequest> validator)
        {
            _requestHandler = requestHandler;
            _validator = validator;
        }

        public async Task<TResponse> HandleAsync(TRequest request)
        {
            var result = await _validator.ValidateAsync(request);

            if (!result.IsSuccessful)
            {
                throw new Exception(result.ErrorMessage);
            }

            return await _requestHandler.HandleAsync(request);
        }
    }

    public class AsyncRequestHandlerValidationDecorator<TRequest> : IAsyncRequestHandler<TRequest>
        where TRequest : IRequest
    {
        private readonly IAsyncRequestHandler<TRequest> _requestHandler;
        private readonly IAsyncRequestValidator<TRequest> _validator;

        public AsyncRequestHandlerValidationDecorator(IAsyncRequestHandler<TRequest> requestHandler, IAsyncRequestValidator<TRequest> validator)
        {
            _requestHandler = requestHandler;
            _validator = validator;
        }

        public async Task HandleAsync(TRequest request)
        {
            var result = await _validator.ValidateAsync(request);

            if (!result.IsSuccessful)
            {
                throw new Exception(result.ErrorMessage);
            }

            await _requestHandler.HandleAsync(request);
        }
    }
}