using SimpleWebApi.Infrastructure.Validation;

namespace SimpleWebApi.Features.Apple.SimpleGetWithValidation
{
    public class Validator : IApiValidator<Request>
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