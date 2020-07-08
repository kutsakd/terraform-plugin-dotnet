namespace NETCore.Terraform.SDK.Schema
{
    /// <summary>
    /// Describes embedding one block in another.
    /// </summary>
    public class TerraformNestedBlock
    {
        /// <summary>
        /// Gets or sets the description of the nested block.
        /// </summary>
        public TerraformBlock Block { get; set; }

        /// <summary>
        /// Gets or sets the nesting mode for the child block, which determines how
        /// many instances of the block are allowed, how many labels it expects, and
        /// how the resulting data will be converted into a data structure.
        /// </summary>
        public TerraformNestingMode Mode { get; set; }

        /// <summary>
        /// Gets or sets, for the <see cref="TerraformNestingMode.List"/> and <see cref="TerraformNestingMode.Set"/>
        /// nesting modes, lower limit on the number of child blocks allowed of the given type.
        /// </summary>
        /// <remarks>If this and <see cref="MaxItems"/> property values ​​are left at zero,
        /// no limit is applied. This and <see cref="MaxItems"/> property are ignored for
        /// other nesting modes and must both be left at zero.</remarks>
        public int MinItems { get; set; }

        /// <summary>
        /// Gets or sets, for the <see cref="TerraformNestingMode.List"/> and <see cref="TerraformNestingMode.Set"/>
        /// nesting modes, upper limit on the number of child blocks allowed of the given type.
        /// </summary>
        /// <remarks>If this and <see cref="MinItems"/> property values ​​are left at zero,
        /// no limit is applied. This and <see cref="MinItems"/> property are ignored for
        /// other nesting modes and must both be left at zero.</remarks>
        public int MaxItems { get; set; }
    }
}
