namespace SimpleWebApi.Infrastructure.Decorators
{
    public interface IValidator<TRequest>
    {
        ValidationResult Validate(TRequest request);
    }
}