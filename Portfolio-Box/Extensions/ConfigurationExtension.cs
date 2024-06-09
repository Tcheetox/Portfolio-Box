using System;
using Microsoft.Extensions.Configuration;

namespace Portfolio_Box.Extensions
{
    public static class ConfigurationExtension
    {
        public static string GetBasePath(this IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            return configuration.GetValue<string>("Hosting:BasePath") ?? string.Empty;
        }

        public static string GetMediaBasePath(this IConfiguration configuration) => configuration.GetBasePath() + "/media/";
    }
}
