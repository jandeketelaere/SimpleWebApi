using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleWebApi.Infrastructure.Validation
{
    public class ValidationDecorator<TRequest, TResponse> : IRequestHandlerDecorator<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IServiceProvider _services;

        public ValidationDecorator(IServiceProvider services)
        {
            _services = services;
        }

        public async Task<ApiResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            var validators =
                _services.GetServices<IValidator<TRequest>>()
                    .OrderBy(v => v.Order)
                    .ToList();

            if (validators.Any())
            {
                foreach (var validator in validators)
                {
                    var result = validator.Validate(request);

                    if (result.IsFailure)
                        return ApiResult<TResponse>.Fail(result.HttpStatusCode, result.ErrorMessage);
                }
            }

            return await next();
        }
    }
}