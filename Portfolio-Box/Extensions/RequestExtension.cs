using System;
using Microsoft.AspNetCore.Http;

namespace Portfolio_Box.Extensions
{
    public static class RequestExtension
    {
        public static bool IsAjaxRequest(this HttpRequest request)
            => request is null
            ? throw new ArgumentNullException(nameof(request))
            : request.Headers != null && request.Headers.XRequestedWith == "XMLHttpRequest";
    }
}
