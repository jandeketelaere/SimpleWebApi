using SimpleWebApi.Entities;
using SimpleWebApi.Infrastructure.Validation;
using System.Linq;

namespace SimpleWebApi.Features.Apple.SimpleGetWithMultipleValidations
{
    public class SecondValidator : IValidator<Request>
    {
        private readonly SimpleWebApiContext _context;

        public int Priority => 2;

        public SecondValidator(SimpleWebApiContext context)
        {
            _context = context;
        }

        public ValidationResult Validate(Request request)
        {
            if (_context.Apples.Any(a => a.Id == request.Id))
                return ValidationResult.BadRequest($"An apple with Id {request.Id} already exists");

            return ValidationResult.Ok();
        }
    }
}