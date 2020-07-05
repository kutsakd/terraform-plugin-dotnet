using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.IO.Compression;

namespace NETCore.Terraform.SDK
{
    /// <summary>
    /// Provides convenience methods for creating instances of <see cref="IHostBuilder"/> with pre-configured defaults.
    /// </summary>
    public static class Plugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HostBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <returns>The initialized <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateDefaultBuilder()
        {
            var builder = new HostBuilder();

            builder.UseContentRoot(Directory.GetCurrentDirectory());
            builder.ConfigureHostConfiguration(configuration =>
            {
                configuration.AddEnvironmentVariables(prefix: "TF_");
            });

            builder.ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddDebug();
                logging.AddEventSourceLogger();
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            });

            builder.ConfigureServices((context, services) =>
            {
                services.AddGrpc(options =>
                {
                    options.EnableDetailedErrors = context.HostingEnvironment.IsDevelopment();
                    options.ResponseCompressionLevel = CompressionLevel.Fastest;
                });
            });

            return builder.UseDefaultServiceProvider((context, options) =>
            {
                var isDevelopment = context.HostingEnvironment.IsDevelopment();
                options.ValidateScopes = isDevelopment;
                options.ValidateOnBuild = isDevelopment;
            });
        }
    }
}
