using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleWebApi.Infrastructure.Validation
{
    public class ValidationDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _handler;
        private readonly IServiceProvider _services;

        public ValidationDecorator(IRequestHandler<TRequest, TResponse> handler, IServiceProvider services)
        {
            _handler = handler;
            _services = services;
        }

        public async Task<ApiResult<TResponse>> Handle(TRequest request)
        {
            var validators =
                _services.GetServices<IApiValidator<TRequest>>()
                .OrderBy(v => v.Priority)
                .ToList();

            if (validators.Any())
            {
                foreach(var validator in validators)
                {
                    var result = validator.Validate(request);

                    if (result.IsFailure)
                        return ApiResult<TResponse>.Fail(result.HttpStatusCode, result.ErrorMessage);
                }
            }

            return await _handler.Handle(request);
        }
    }
}