using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NETCore.Terraform.SDK.Abstractions;
using System;
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
            builder.ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddDebug();
                logging.AddEventSourceLogger();
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            });

            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IMetadataProvider, DefaultMetadataProvider>();

                services.AddGrpc(options =>
                {
                    options.EnableDetailedErrors = context.HostingEnvironment.IsDevelopment();
                    options.ResponseCompressionLevel = CompressionLevel.Fastest;
                });
            });

            builder.ConfigureWebHost(hostBuilder =>
            {
                hostBuilder.UseKestrel(options =>
                {
                    options.ConfigureListeners();

                    // If the client is configured using AutoMTLS, the certificate will be here,
                    // and we need to generate our own in response.
                    options.ConfigureHttps();
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
