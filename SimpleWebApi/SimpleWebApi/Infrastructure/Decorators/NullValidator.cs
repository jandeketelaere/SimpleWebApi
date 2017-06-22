using FluentValidation;

namespace SimpleWebApi.Infrastructure.Decorators
{
    public class NullValidator<TRequest> : AbstractValidator<TRequest>
    {
    }
}