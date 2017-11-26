namespace SimpleWebApi.Infrastructure.Validation
{
    public interface IApiValidator<TRequest>
    {
        ValidationResult Validate(TRequest request);
        int Priority { get; }
    }
}