namespace SimpleWebApi.Infrastructure.Validation
{
    public interface IValidator<TRequest>
    {
        ValidationResult Validate(TRequest request);
    }
}