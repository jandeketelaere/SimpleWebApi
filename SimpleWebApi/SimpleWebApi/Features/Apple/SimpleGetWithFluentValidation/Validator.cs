using FluentValidation;
using SimpleWebApi.Infrastructure.Validation;

namespace SimpleWebApi.Features.Apple.SimpleGetWithFluentValidation
{
    public class Validator : IApiValidator<Request>
    {
        public int Priority => 1;

        public class FluentValidator : AbstractValidator<Request>
        {
            public FluentValidator()
            {
                RuleFor(x => x.Id).NotEqual(1);
            }
        }

        public ValidationResult Validate(Request request) => FluentApiValidator.Validate(request, new FluentValidator());
    }
}