using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace Vetris.Report.Core.Configurations
{
    public static class AppConfigurations
    {
        private static readonly ConcurrentDictionary<string, IConfigurationRoot> ConfigurationCache;

        static AppConfigurations()
        {
            ConfigurationCache = new ConcurrentDictionary<string, IConfigurationRoot>();
        }

        public static IConfigurationRoot Get(string path)
        {
            var cacheKey = path;
            return ConfigurationCache.GetOrAdd(
                cacheKey,
                _ => BuildConfiguration(path)
            );
        }

        private static IConfigurationRoot BuildConfiguration(string path)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

           
            builder = builder.AddEnvironmentVariables();


            return builder.Build();
        }
    }
}
