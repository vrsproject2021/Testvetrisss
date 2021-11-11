using AutoMapper;
using AutoWrapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using Vetris.Report.Core.Automapping;
using Vetris.Report.Core.Configurations;
using Vetris.Report.Core.Dependency;
using Vetris.Report.Core.Extensions;
using Vetris.Report.Server.Authorization;
using Vetris.Report.Server.Extensions;

namespace Vetris.Report.Server
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";
        private const string _apiVersion = "v1";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            // Automapper configuration
            services.AddAutoMapper(c =>
            {
                c.AddProfile(typeof(CustomMapper));
            });
            //Configure CORS for angular UI

            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            Configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );
            string[] assembliesToBeScanned = new string[] { "Vetris.Report.Server", "Vetris.Report.Core", "Vetris.Report.Service", "Vetris.Report.DataAccess" };
            services.AddServicesOfType<ISingletonDependency>(assembliesToBeScanned);
            services.AddServicesOfType<ITransientDependency>(assembliesToBeScanned);

            // configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            services.AddControllers().AddNewtonsoftJson();
            //services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAppFolders appFolders)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            ServiceActivator.Configure(app.ApplicationServices);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(_defaultCorsPolicyName); // Enable CORS!
            app.UseStaticFiles();
            //app.UseExceptionHandler(new ExceptionHandlerOptions
            //{
            //    ExceptionHandler = new GlobalErrorHandling().Invoke
            //});
           
            //
            // Serve reports store folder for static link
            //
            if (!string.IsNullOrEmpty(Configuration["FileServer:reportspath"]))
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Configuration["FileServer:reportspath"]),
                    RequestPath = new PathString(Configuration["reportfiles"])
                });
            }

            app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions
            {
                ApiVersion = _apiVersion,
                ShowIsErrorFlagForSuccessfulResponse = true
            });

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
            });

            if (string.IsNullOrWhiteSpace(env.WebRootPath))
            {
                env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
            appFolders.TempFileDownloadFolder = Path.Combine(env.WebRootPath, "temp/download");
            appFolders.ReportsFolder = Configuration["FileServer:reportspath"];
            appFolders.TempReportsFolder = Path.Combine(appFolders.ReportsFolder, "temporary");


            if (!Directory.Exists(appFolders.TempFileDownloadFolder)) Directory.CreateDirectory(appFolders.TempFileDownloadFolder);
            if (!Directory.Exists(appFolders.TempReportsFolder)) Directory.CreateDirectory(appFolders.TempReportsFolder);
        }
    }
}
