
namespace TheBoxSoftware.Documentation.Exporting
{
    /// <summary>
    /// Describes an issue at various points of the export process.
    /// </summary>
    public sealed class Issue
    {
        private string _description;

        /// <summary>
        /// Gets or sets a description of the issue.
        /// </summary>
        public string Description
        {
            get => _description;
            set => _description = value;
        }
    }
}