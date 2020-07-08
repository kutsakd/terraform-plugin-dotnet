namespace NETCore.Terraform.SDK.Schema
{
    /// <summary>
    /// Describes the configuration attribute, within a block.
    /// </summary>
    public class TerraformAttribute
    {
        /// <summary>
        /// Gets or sets an English-language description of the usage of the attribute.
        /// </summary>
        /// <remarks>
        /// A description should be concise and use only one or two sentences,
        /// leaving full definition to longer-form documentation defined elsewhere.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the attribute description format.
        /// </summary>
        public TerraformDescriptionKind DescriptionKind { get; set; } = TerraformDescriptionKind.Text;

        /// <summary>
        /// Gets or sets an indication that an omitted or null value is permitted.
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// Gets or sets an indication that the value comes from the provider rather than from configuration.
        /// If combined with <see cref="Optional"/>, then the config may optionally provide an overridden value.
        /// </summary>
        public bool Computed { get; set; }

        /// <summary>
        /// Gets or sets an indication that the attribute may contain sensitive information.
        /// </summary>
        /// <remarks>
        /// At present nothing is done with this information, but callers are encouraged to set it where
        /// appropriate so that it may be used in the future to help Terraform mask sensitive information.
        /// </remarks>
        public bool Sensitive { get; set; }

        /// <summary>
        /// Gets or sets an indication of whether the block has been marked as deprecated
        /// in the provider and usage should be discouraged.
        /// </summary>
        public bool Deprecated { get; set; }
    }
}
