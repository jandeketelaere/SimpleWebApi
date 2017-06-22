using FluentValidation;

namespace SimpleWebApi.Infrastructure
{
    public class NullValidator<TRequest> : AbstractValidator<TRequest>
    {
    }
}