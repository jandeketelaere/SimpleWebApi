namespace SimpleWebApi.Infrastructure.Logging
{
    public interface IApiLogger
    {
        void LogInformation(string message, LoggingType loggingType);
        void LogError(string message, LoggingType loggingType);
    }
}