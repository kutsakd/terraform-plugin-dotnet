using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.AspNetCore.Hosting
{
    internal static class KestrelServerOptionsExtensions
    {
        public static KestrelServerOptions ConfigureHttps(this KestrelServerOptions options)
        {
            var certificateContent = Environment.GetEnvironmentVariable("PLUGIN_CLIENT_CERT");
            var certificate = ParseClientCertificate(certificateContent);
            if (certificate != null)
            {
                options.ConfigureHttpsDefaults(connectionOptions =>
                {
                    connectionOptions.SslProtocols = SslProtocols.Tls12;
                    connectionOptions.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                    connectionOptions.ClientCertificateValidation = (clientCertificate, chain, _) =>
                    {
                        return clientCertificate.Thumbprint == certificate.Thumbprint;
                    };
                });
            }

            return options;
        }

        public static KestrelServerOptions ConfigureListeners(this KestrelServerOptions options)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                options.Listen(IPAddress.Loopback, 0, listenOptions => listenOptions.UseHttps());
                options.ConfigureEndpointDefaults(listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
            }
            else
            {
                var socketPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                options.ListenUnixSocket(socketPath, listenOptions => listenOptions.UseHttps());
            }

            return options;
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
