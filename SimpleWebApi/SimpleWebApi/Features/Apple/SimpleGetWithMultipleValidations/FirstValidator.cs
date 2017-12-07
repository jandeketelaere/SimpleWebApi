using SimpleWebApi.Infrastructure.Validation;

namespace SimpleWebApi.Features.Apple.SimpleGetWithMultipleValidations
{
    public class FirstValidator : IValidator<Request>
    {
        public int Order => 1;

        public ValidationResult Validate(Request request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return ValidationResult.BadRequest("Name is required");

            return ValidationResult.Ok();
        }
    }
}