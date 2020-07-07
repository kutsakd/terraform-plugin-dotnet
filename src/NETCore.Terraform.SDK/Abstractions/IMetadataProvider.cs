using Microsoft.AspNetCore.Http.Features;

namespace NETCore.Terraform.SDK.Abstractions
{
    public interface IMetadataProvider
    {
        PluginMetadata GetMetadata(IFeatureCollection serverFeatures);
    }
}
