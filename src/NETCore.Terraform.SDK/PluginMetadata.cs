using System.Security.Cryptography.X509Certificates;

namespace NETCore.Terraform.SDK
{
    public class PluginMetadata
    {
        public int TerraformProtocolVersion { get; set; }
        public int ProtocolVersion { get; set; }
        public string ListenerType { get; set; }
        public string ListenerPath { get; set; }
        public string ProtocolType { get; set; }
        public X509Certificate ListenerCertificate { get; set; }
    }
}
