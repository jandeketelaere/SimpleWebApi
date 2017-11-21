using Microsoft.AspNetCore.Http;
using SimpleWebApi.Infrastructure.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IApiLogger logger)
        {
            logger.LogInformation(await GetRequestLogging(context), LoggingType.Request);

            var bodyStream = context.Response.Body;
            var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            logger.LogInformation(await GetResponseLogging(context.Response, context.Request.Path, responseBodyStream, bodyStream), LoggingType.Response);
        }

        public async Task<string> GetRequestLogging(HttpContext context)
        {
            var request = context.Request;
            var requestBodyStream = new MemoryStream();
            var originalRequestBody = request.Body;

            await request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            var requestBodyText = await new StreamReader(requestBodyStream).ReadToEndAsync();

            var requestLog = $"Method: {request.Method}, Url: {request.Path}";
            if (!string.IsNullOrWhiteSpace(requestBodyText))
                requestLog += $", Body: {requestBodyText}";

            requestLog += $", Client: {context.Connection.RemoteIpAddress}";

            requestBodyStream.Seek(0, SeekOrigin.Begin);
            request.Body = requestBodyStream;

            return requestLog;
        }

        public async Task<string> GetResponseLogging(HttpResponse response, string path, MemoryStream responseBodyStream, Stream bodyStream)
        {
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(responseBodyStream).ReadToEnd();

            var responseLog = $"Code: {response.StatusCode}";
            if (!string.IsNullOrWhiteSpace(responseBody))
            {
                responseLog += ", Body: ";
                if (!IsBodyIgnoredFromResponseLogging(path))
                    responseLog += $"{ responseBody }";
                else
                    responseLog += "Ignored";
            }

            responseBodyStream.Seek(0, SeekOrigin.Begin);

            if (response.StatusCode != 204)
                await responseBodyStream.CopyToAsync(bodyStream);

            return responseLog;
        }

        private bool IsBodyIgnoredFromResponseLogging(string path)
        {
            var paths = new List<string>
            {
            };

            return paths.Any(x => x == path);
        }
    }
}