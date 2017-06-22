using FluentValidation;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure.Decorators
{
    public class RequestHandlerValidator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _requestHandler;
        private readonly IValidator<TRequest> _validator;

        public RequestHandlerValidator(IRequestHandler<TRequest, TResponse> requestHandler, IValidator<TRequest> validator)
        {
            _requestHandler = requestHandler;
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request)
        {
            var result = await _validator.ValidateAsync(request);

            if (result.Errors.Any())
            {
                throw new ValidationException(result.Errors);
            }

            return await _requestHandler.Handle(request);
        }
    }

    public class RequestHandlerValidator<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        private readonly IRequestHandler<TRequest> _requestHandler;
        private readonly IValidator<TRequest> _validator;

        public RequestHandlerValidator(IRequestHandler<TRequest> requestHandler, IValidator<TRequest> validator)
        {
            _requestHandler = requestHandler;
            _validator = validator;
        }

        public async Task Handle(TRequest request)
        {
            var result = await _validator.ValidateAsync(request);

            if (result.Errors.Any())
            {
                throw new ValidationException(result.Errors);
            }

            await _requestHandler.Handle(request);
        }
    }
}