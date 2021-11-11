using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;

namespace Vetris.Report.Server
{
    public class Program
    {
        private static int? listenPort = null;

        public static void Main(string[] args)
        {


            var configuration = new ConfigurationBuilder()
           .AddEnvironmentVariables()
           .AddCommandLine(args)
           .AddJsonFile("appsettings.json")
           .Build();
            string strlistenPort = configuration["App:Port"];
            if (!string.IsNullOrEmpty(strlistenPort))
                listenPort = Convert.ToInt32(configuration["App:Port"]);

            var log4netRepository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.Config.XmlConfigurator.Configure(log4netRepository, new FileInfo("log4net.config"));

            CreateHostBuilder(args).Build().Run();


        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        if (listenPort.HasValue)
                            serverOptions.ListenAnyIP(listenPort.Value);
                        serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(60);
                        serverOptions.AddServerHeader = false;
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
