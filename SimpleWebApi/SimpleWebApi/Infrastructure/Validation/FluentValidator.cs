using FluentValidation;
using System.Linq;

namespace SimpleWebApi.Infrastructure.Validation
{
    public static class FluentApiValidator
    {
        public static ValidationResult Validate<Request>(Request request, IValidator<Request> validator)
        {
            var result = validator.Validate(request);

            if (!result.IsValid)
                return ValidationResult.BadRequest(string.Join(". ", result.Errors.Select(e => e.ErrorMessage)));

            return ValidationResult.Ok();
        }
    }
}