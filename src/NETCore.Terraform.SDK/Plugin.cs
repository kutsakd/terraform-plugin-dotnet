using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.IO.Compression;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

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
        public static IHostBuilder CreateDefaultBuilder() => CreateDefaultBuilder(new PluginOptions());

        /// <summary>
        /// Initializes a new instance of the <see cref="HostBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <param name="options">Options for the current plugin and gRPC server.</param>
        /// <returns>The initialized <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateDefaultBuilder(PluginOptions options)
        {
            if (string.IsNullOrEmpty(options.MagicCookieKey) || string.IsNullOrEmpty(options.MagicCookieValue))
            {
                throw new InvalidOperationException($"Misconfigured {nameof(PluginOptions)} given to serve this plugin: " +
                    "no magic cookie key or value was set. Please notify the plugin author and report this as a bug.");
            }

            if (Environment.GetEnvironmentVariable(options.MagicCookieKey) != options.MagicCookieValue)
            {
                throw new InvalidOperationException("This binary is a plugin. These are not meant to be executed directly. " +
                    "Please execute the program that consumes these plugins, which will load any plugins automatically.");
            }

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

            var certificateContent = Environment.GetEnvironmentVariable("PLUGIN_CLIENT_CERT");
            var certificate = ParseClientCertificate(certificateContent);
            if (certificate != null)
            {
                builder.ConfigureWebHost(hostBuilder => hostBuilder.ConfigureKestrel(options =>
                {
                    options.ConfigureHttpsDefaults(connectionOptions =>
                    {
                        connectionOptions.SslProtocols = SslProtocols.Tls12;
                        connectionOptions.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                        connectionOptions.ClientCertificateValidation = (certificate, chain, _) =>
                        {
                            return certificate.Thumbprint == certificate.Thumbprint;
                        };
                    });
                }));
            }

            Console.WriteLine("1|5|tcp|127.0.0.1:5001|grpc|");
            return builder.UseDefaultServiceProvider((context, options) =>
            {
                var isDevelopment = context.HostingEnvironment.IsDevelopment();
                options.ValidateScopes = isDevelopment;
                options.ValidateOnBuild = isDevelopment;
            });
        }

        private static X509Certificate2 ParseClientCertificate(string certificateContent)
        {
            if (!string.IsNullOrWhiteSpace(certificateContent))
            {
                var certificateBlocks = certificateContent.Split('-', StringSplitOptions.RemoveEmptyEntries);
                if (certificateBlocks.Length > 1)
                {
                    var certificateBytes = Convert.FromBase64String(certificateBlocks[1]);
                    return new X509Certificate2(certificateBytes);
                }
            }

            return null;
        }
    }
}
