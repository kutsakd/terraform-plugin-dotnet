namespace NETCore.Terraform.SDK
{
    /// <summary>
    /// Describes the options used to configure clients and servers.
    /// </summary>
    public class PluginOptions
    {
        /// <summary>
        /// Gets the protocol version assumed for legacy clients that don't specify
        /// a particular version during their handshake. This is the version used
        /// when Terraform 0.10 and 0.11 launch plugins that were built with support
        /// for both versions 4 and 5, and must stay unchanged at 4 until we intentionally
        /// build plugins that are not compatible with 0.10 and 0.11.
        /// </summary>
        public const int DefaultProtocolVersion = 4;

        public string MagicCookieKey => "TF_PLUGIN_MAGIC_COOKIE";
        public string MagicCookieValue => "d602bf8f470bc67ca7faa0386276bbdd4330efaf76d1a219cb4d6991ca9872b2";

        /// <summary>
        /// Gets the version that must match between TF core and TF plugins.
        /// This should be bumped whenever a change happens in one or the other
        /// that makes it so that they can't safely communicate.
        /// </summary>
        public int ProtocolVersion => DefaultProtocolVersion;
    }
}
