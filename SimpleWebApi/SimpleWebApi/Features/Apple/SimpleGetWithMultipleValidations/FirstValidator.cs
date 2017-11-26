using SimpleWebApi.Infrastructure.Validation;

namespace SimpleWebApi.Features.Apple.SimpleGetWithMultipleValidations
{
    public class FirstValidator : IApiValidator<Request>
    {
        public int Priority => 1;

        public ValidationResult Validate(Request request)
        {
            if (request.Id == 0)
                return ValidationResult.BadRequest("Id cannot be 0");

            return ValidationResult.Ok();
        }
    }
}