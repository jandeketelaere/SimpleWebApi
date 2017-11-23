using System.Net;

namespace SimpleWebApi.Infrastructure.Decorators
{
    public class ValidationResult
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorMessage { get; }

        public HttpStatusCode HttpStatusCode { get; }
        public string Error;

        internal ValidationResult(bool isSuccessful, HttpStatusCode httpStatusCode, string errorMessage)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
            HttpStatusCode = httpStatusCode;
        }

        public static ValidationResult Ok()
        {
            return new ValidationResult(true, HttpStatusCode.OK, null);
        }
        
        public static ValidationResult Forbidden(string errorMessage)
        {
            return new ValidationResult(false, HttpStatusCode.Forbidden, errorMessage);
        }

        public static ValidationResult Unauthorized(string errorMessage)
        {
            return new ValidationResult(false, HttpStatusCode.Unauthorized, errorMessage);
        }

        public static ValidationResult BadRequest(string errorMessage)
        {
            return new ValidationResult(false, HttpStatusCode.BadRequest, errorMessage);
        }

        public static ValidationResult NotFound(string errorMessage)
        {
            return new ValidationResult(false, HttpStatusCode.NotFound, errorMessage);
        }

        public static ValidationResult InternalServerError(string errorMessage)
        {
            return new ValidationResult(false, HttpStatusCode.InternalServerError, errorMessage);
        }
    }
}