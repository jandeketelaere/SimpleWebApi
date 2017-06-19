using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure
{
    public class NullAsyncRequestValidator<TRequest> : IAsyncRequestValidator<TRequest>
    {
        public async Task<ValidationResult> ValidateAsync(TRequest request)
        {
            return new ValidationResult
            {
                IsSuccessful = true
            };
        }
    }
}