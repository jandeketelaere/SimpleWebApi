namespace SimpleWebApi.Infrastructure.Validation
{
    public interface IValidator<in TRequest>
    {
        ValidationResult Validate(TRequest request);
        int Order { get; }
    }
}