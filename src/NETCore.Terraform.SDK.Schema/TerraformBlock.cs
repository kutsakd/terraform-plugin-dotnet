using System.Collections.Generic;

namespace NETCore.Terraform.SDK.Schema
{
    /// <summary>
    /// Describes the configuration block.
    /// </summary>
    /// <remarks>
    /// This is a logical grouping construct, though it happens to map directly onto the physical
    /// block syntax of Terraform's native configuration syntax. It may be a more a matter of
    /// convention in other syntaxes, such as JSON. When converted to a value, a Block always
    /// becomes an instance of an object type derived from its defined attributes and nested blocks.
    /// </remarks>
    public class TerraformBlock
    {
        /// <summary>
        /// Gets any attributes that may appear directly inside the block.
        /// </summary>
        public IDictionary<string, TerraformAttribute> Attributes { get; }
            = new Dictionary<string, TerraformAttribute>();

        /// <summary>
        /// Gets any nested block types that may appear directly inside the block.
        /// </summary>
        public IDictionary<string, TerraformNestedBlock> NestedBlocks { get; }
            = new Dictionary<string, TerraformNestedBlock>();

        /// <summary>
        /// Gets or sets an English-language description of the usage of the block.
        /// </summary>
        /// <remarks>
        /// A description should be concise and use only one or two sentences,
        /// leaving full definition to longer-form documentation defined elsewhere.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the block description format.
        /// </summary>
        public TerraformDescriptionKind DescriptionKind { get; set; } = TerraformDescriptionKind.Text;

        /// <summary>
        /// Gets or sets an indication of whether the block has been marked as deprecated
        /// in the provider and usage should be discouraged.
        /// </summary>
        public bool Deprecated { get; set; }
    }
}
