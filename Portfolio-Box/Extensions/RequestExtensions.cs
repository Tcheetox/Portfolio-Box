using System;
using Microsoft.AspNetCore.Http;

namespace Portfolio_Box.Extensions
{
    public static class RequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return request.Headers != null && request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
