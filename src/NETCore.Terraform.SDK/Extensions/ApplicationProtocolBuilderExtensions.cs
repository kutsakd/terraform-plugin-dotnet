using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.Terraform.SDK.Abstractions;
using NETCore.Terraform.SDK.Protocols;
using System;

namespace NETCore.Terraform.SDK.Extensions
{
    public static class ApplicationProtocolBuilderExtensions
    {
        public static IApplicationBuilder UseTerraform(this IApplicationBuilder applicationBuilder)
        {
            var services = applicationBuilder.ApplicationServices;
            var metadataProvider = services.GetRequiredService<IMetadataProvider>();
            var applicationLifetime = services.GetRequiredService<IHostApplicationLifetime>();

            applicationLifetime.ApplicationStarted.Register(() =>
            {
                var metadata = metadataProvider.GetMetadata(applicationBuilder.ServerFeatures);
                var certificate = metadata.ListenerCertificate?.GetPublicKeyString();

                // Output the address and service name to stdout so that the client can bring it up.
                Console.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}",
                    metadata.TerraformProtocolVersion, metadata.ProtocolVersion,
                    metadata.ListenerType, metadata.ListenerPath,
                    metadata.ProtocolType, certificate);
            });

            applicationBuilder.UseRouting();
            return applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ProviderService>();
                endpoints.MapGrpcService<ProvisionerService>();
            });
        }
    }
}
