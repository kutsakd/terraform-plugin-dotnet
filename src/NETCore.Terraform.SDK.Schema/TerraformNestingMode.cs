namespace NETCore.Terraform.SDK.Schema
{
    /// <summary>
    /// Provides an enumeration of modes for nesting blocks inside other blocks.
    /// </summary>
    public enum TerraformNestingMode
    {
        /// <summary>
        /// Indicates that only a single instance of a given block type
        /// is permitted, with no labels, and its content should be provided
        /// directly as an object value.
        /// </summary>
        Single = 1,

        /// <summary>
        /// Indicates that only a single instance of a given block type is permitted,
        /// with no labels, and additonally guarantees that its result will never be
        /// null, even if the block is absent, and instead the nested attributes and
        /// blocks will be treated as absent in that case. (Any required attributes or
        /// blocks within the nested block are not enforced unless the block is explicitly
        /// present in the configuration, so they are all effectively optional when the
        /// block is not present)
        /// </summary>
        Group,

        /// <summary>
        /// Indicates that multiple blocks of the given type are permitted,
        /// with no labels, and that their corresponding objects should be
        /// provided in a list.
        /// </summary>
        List,

        /// <summary>
        /// Indicates that multiple blocks of the given type are permitted
        /// with no labels, and that their corresponding objects should be
        /// provided in a set.
        /// </summary>
        Set,

        /// <summary>
        /// Indicates that multiple blocks of the given type are permitted,
        /// each with a single label, and that their corresponding objects
        /// should be provided in a map whose keys are the labels.
        /// </summary>
        Map
    }
}
