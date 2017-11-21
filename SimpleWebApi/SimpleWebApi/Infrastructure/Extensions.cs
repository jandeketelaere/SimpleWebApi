using System.Net;

namespace SimpleWebApi.Infrastructure
{
    public static class Extensions
    {
        public static bool IsSuccessStatusCode(this HttpStatusCode statusCode) => ((int)statusCode >= 200) && ((int)statusCode <= 299);
    }
}