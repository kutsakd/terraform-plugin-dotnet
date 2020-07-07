using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using NETCore.Terraform.SDK.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace NETCore.Terraform.SDK
{
    public class DefaultMetadataProvider : IMetadataProvider
    {
        // TerraformProtocolVersion is the ProtocolVersion of the plugin system itself.
        // We will increment this whenever we change any protocol behavior. This
        // will invalidate any prior plugins but will at least allow us to iterate
        // on the core in a safe way. We will do our best to do this very
        // infrequently.
        public const int TerraformProtocolVersion = 1;

        public PluginMetadata GetMetadata(IFeatureCollection serverFeatures)
        {
            var metadata = new PluginMetadata
            {
                TerraformProtocolVersion = TerraformProtocolVersion,
                ProtocolType = "grpc"
            };

            SetListenerConfiguration(metadata, serverFeatures);
            SetProtocolConfiguration(metadata);
            return metadata;
        }

        protected void SetListenerConfiguration(PluginMetadata metadata, IFeatureCollection serverFeatures)
        {
            var serverAddressesFeature = serverFeatures.Get<IServerAddressesFeature>();
            var serverAddresses = serverAddressesFeature.Addresses.Select(addr =>
            {
                var addressBlocks = addr.Split("://", count: 2);
                if (addressBlocks.Length > 1 && addressBlocks[1].StartsWith("unix:"))
                {
                    addressBlocks[0] += "-unix";
                    addressBlocks[1] = addressBlocks[1].Substring("unix:".Length);
                    return new Uri(string.Join("://", addressBlocks));
                }

                return new Uri(addr);
            });

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var listenerAddress = serverAddresses.First(addr => addr.Scheme == "https");
                metadata.ListenerType = listenerAddress.Scheme;
                metadata.ListenerPath = listenerAddress.Authority;
                return;
            }

            var unixAddress = serverAddresses.First(addr => addr.Scheme == "https-unix");
            metadata.ListenerType = "unix";
            metadata.ListenerPath = unixAddress.AbsolutePath;
        }

        protected void SetProtocolConfiguration(PluginMetadata metadata)
        {
            var clientVersionsStr = Environment.GetEnvironmentVariable("PLUGIN_PROTOCOL_VERSIONS");
            if (!string.IsNullOrWhiteSpace(clientVersionsStr))
            {
                var clientVersionCollection = new Collection<int>();
                foreach (var clientVersionStr in clientVersionsStr.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!int.TryParse(clientVersionStr, out var clientVersion))
                    {
                        throw new InvalidOperationException(
                            $"Terraform client sent invalid plugin version: {clientVersionStr}");
                    }

                    clientVersionCollection.Add(clientVersion);
                }

                if (clientVersionCollection.Contains(5))
                {
                    metadata.ProtocolVersion = 5;
                    return;
                }
            }

            // Return the lowest version as the fallback.
            // Since we iterated over all the versions in reverse order above, these
            // values are from the lowest version number plugins. This allows serving
            // the oldest version of our plugins to a legacy client that did not send
            // a PLUGIN_PROTOCOL_VERSIONS list.
            metadata.ProtocolVersion = PluginOptions.DefaultProtocolVersion;
        }
    }
}
