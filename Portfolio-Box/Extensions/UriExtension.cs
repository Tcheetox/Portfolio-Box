using System;
using System.Linq;

namespace Portfolio_Box.Extensions;

public static class UriExtension
{
	public static Uri Append(this Uri uri, params string[] paths)
	{
		ArgumentNullException.ThrowIfNull(uri);
		return new Uri(paths.Aggregate(uri.AbsoluteUri, (current, path) => string.Format("{0}/{1}", current.TrimEnd('/'), path.TrimStart('/'))));
	}
}