using SimpleWebApi.Entities;
using SimpleWebApi.Infrastructure.Validation;
using System.Linq;

namespace SimpleWebApi.Features.Apple.SimpleGetWithMultipleValidations
{
    public class SecondValidator : IValidator<Request>
    {
        private readonly SimpleWebApiContext _context;

        public int Order => 2;

        public SecondValidator(SimpleWebApiContext context)
        {
            _context = context;
        }

        public ValidationResult Validate(Request request)
        {
            if (_context.Apples.Any(a => a.Name == request.Name))
                return ValidationResult.BadRequest($"An apple with name {request.Name} already exists");

            return ValidationResult.Ok();
        }
    }
}