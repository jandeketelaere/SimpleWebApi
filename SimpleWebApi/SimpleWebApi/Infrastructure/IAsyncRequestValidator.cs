using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure
{
    public interface IAsyncRequestValidator<TRequest>
    {
        Task<ValidationResult> ValidateAsync(TRequest request);
    }

    public class ValidationResult
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
    }
}