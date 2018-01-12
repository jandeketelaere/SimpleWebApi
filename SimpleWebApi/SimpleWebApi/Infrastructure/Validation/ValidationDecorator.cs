using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure.Validation
{
    public class ValidationDecorator<TRequest, TResponse> : IRequestHandlerDecorator<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationDecorator(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<ApiResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                foreach (var validator in _validators.OrderBy(v => v.Order))
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