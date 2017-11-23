using SimpleWebApi.Infrastructure.Decorators;

namespace SimpleWebApi.Features.Apple.SimpleGetWithValidation
{
    public class Validator : IValidator<Request>
    {
        public ValidationResult Validate(Request request)
        {
            if (request.Id == 0)
                return ValidationResult.BadRequest("Id cannot be 0");

            return ValidationResult.Ok();
        }
    }
}