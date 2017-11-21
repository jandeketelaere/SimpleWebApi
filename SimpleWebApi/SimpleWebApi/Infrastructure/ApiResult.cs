using System.Net;

namespace SimpleWebApi.Infrastructure
{
    public class ApiResult<TValue>
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorMessage { get; }

        public HttpStatusCode HttpStatusCode { get; }
        public TValue Value { get; }
        public string Error;

        internal ApiResult(TValue value, bool isSuccessful, HttpStatusCode httpStatusCode, string errorMessage)
        {
            Value = value;
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
            HttpStatusCode = httpStatusCode;
        }

        public static ApiResult<TValue> Ok(TValue value)
        {
            return new ApiResult<TValue>(value, true, HttpStatusCode.OK, null);
        }

        public static ApiResult<TValue> Created(TValue value)
        {
            return new ApiResult<TValue>(value, true, HttpStatusCode.Created, null);
        }

        public static ApiResult<TValue> NoContent()
        {
            return new ApiResult<TValue>(default(TValue), true, HttpStatusCode.NoContent, null);
        }

        public static ApiResult<TValue> Forbidden(string errorMessage)
        {
            return new ApiResult<TValue>(default(TValue), false, HttpStatusCode.Forbidden, errorMessage);
        }

        public static ApiResult<TValue> Unauthorized(string errorMessage)
        {
            return new ApiResult<TValue>(default(TValue), false, HttpStatusCode.Unauthorized, errorMessage);
        }

        public static ApiResult<TValue> BadRequest(string errorMessage)
        {
            return new ApiResult<TValue>(default(TValue), false, HttpStatusCode.BadRequest, errorMessage);
        }

        public static ApiResult<TValue> NotFound(string errorMessage)
        {
            return new ApiResult<TValue>(default(TValue), false, HttpStatusCode.NotFound, errorMessage);
        }

        public static ApiResult<TValue> InternalServerError(string errorMessage)
        {
            return new ApiResult<TValue>(default(TValue), false, HttpStatusCode.InternalServerError, errorMessage);
        }

        public static ApiResult<TValue> Fail(HttpStatusCode httpStatusCode, string errorMessage)
        {
            return new ApiResult<TValue>(default(TValue), false, httpStatusCode, errorMessage);
        }
    }

    public class ApiResult
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorMessage { get; }

        public HttpStatusCode HttpStatusCode { get; }
        public string Error;

        internal ApiResult(bool isSuccessful, HttpStatusCode httpStatusCode, string errorMessage)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
            HttpStatusCode = httpStatusCode;
        }

        public static ApiResult Ok()
        {
            return new ApiResult(true, HttpStatusCode.OK, null);
        }

        public static ApiResult Created()
        {
            return new ApiResult(true, HttpStatusCode.Created, null);
        }

        public static ApiResult NoContent()
        {
            return new ApiResult(true, HttpStatusCode.NoContent, null);
        }

        public static ApiResult Forbidden(string errorMessage)
        {
            return new ApiResult(false, HttpStatusCode.Forbidden, errorMessage);
        }

        public static ApiResult Unauthorized(string errorMessage)
        {
            return new ApiResult(false, HttpStatusCode.Unauthorized, errorMessage);
        }

        public static ApiResult BadRequest(string errorMessage)
        {
            return new ApiResult(false, HttpStatusCode.BadRequest, errorMessage);
        }

        public static ApiResult NotFound(string errorMessage)
        {
            return new ApiResult(false, HttpStatusCode.NotFound, errorMessage);
        }

        public static ApiResult InternalServerError(string errorMessage)
        {
            return new ApiResult(false, HttpStatusCode.InternalServerError, errorMessage);
        }

        public static ApiResult Fail(HttpStatusCode httpStatusCode, string errorMessage)
        {
            return new ApiResult(false, httpStatusCode, errorMessage);
        }
    }
}