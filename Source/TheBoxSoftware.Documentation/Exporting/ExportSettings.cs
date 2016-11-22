
namespace TheBoxSoftware.Documentation.Exporting
{
    /// <summary>
    /// Settings which govern where, when, how and all other information regarding
    /// the export of documentation.
    /// </summary>
    public sealed class ExportSettings
    {
        private DocumentSettings _settings;
        private string _title;
        private string _publishDirectory;

        /// <summary>
        /// Initialises a new instance of the ExportSettings class.
        /// </summary>
        public ExportSettings()
        {
        }

        /// <summary>
        /// The export overridden settings for exporting documentation
        /// </summary>
        public DocumentSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        /// <summary>
        /// The user configured title for the documentation
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// The directory where all final output should be copied after export.
        /// </summary>
        public string PublishDirectory
        {
            get { return _publishDirectory; }
            set { _publishDirectory = value; }
        }
    }
}