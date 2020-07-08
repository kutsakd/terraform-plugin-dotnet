namespace NETCore.Terraform.SDK.Schema
{
    /// <summary>
    /// Describes the element description format.
    /// </summary>
    public enum TerraformDescriptionKind
    {
        /// <summary>
        /// Indicates a string is plain-text and requires no processing for display.
        /// </summary>
        Text,

        /// <summary>
        /// Indicates a string is in markdown format and may require additional processing to display.
        /// </summary>
        Markdown
    }
}
