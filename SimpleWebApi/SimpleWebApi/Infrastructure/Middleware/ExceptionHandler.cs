using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure.Middleware
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private static Task HandleException(HttpContext context, Exception ex)
        {
            //todo: log

            var statusCode = GetStatusCode(ex);
            var errorMessages = GetErrorMessages(ex);

            var result = JsonConvert.SerializeObject(new { ErrorMessages = errorMessages });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(result);
        }

        private static HttpStatusCode GetStatusCode(Exception ex)
        {
            if (ex is ValidationException validationException)
            {
                return HttpStatusCode.BadRequest;
            }

            return HttpStatusCode.InternalServerError;
        }

        private static IEnumerable<string> GetErrorMessages(Exception ex)
        {
            if (ex is ValidationException validationException)
            {
                return validationException.Errors.Select(x => x.ErrorMessage);
            }

            return new[] { ex.ToString() };
        }
    }
}