namespace SimpleWebApi.Infrastructure
{
    public class Result<TValue>
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorMessage { get; }
        public TValue Value { get; }

        internal Result(TValue value, bool isSuccessful, string errorMessage)
        {
            Value = value;
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }

        public static Result<TValue> Ok(TValue value)
        {
            return new Result<TValue>(value, true, null);
        }

        public static Result<TValue> Fail(string errorMessage)
        {
            return new Result<TValue>(default(TValue), false, errorMessage);
        }
    }

    public class Result
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorMessage { get; }

        internal Result(bool isSuccessful, string errorMessage)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }

        public static Result Ok()
        {
            return new Result(true, null);
        }

        public static Result Fail(string errorMessage)
        {
            return new Result(false, errorMessage);
        }
    }
}