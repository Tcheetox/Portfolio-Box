using System;
using Microsoft.AspNetCore.Http;

namespace Portfolio_Box.Extensions;

public static class RequestExtension
{
	public static bool IsAjaxRequest(this HttpRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		return request.Headers != null && request.Headers.XRequestedWith == "XMLHttpRequest";
	}
}