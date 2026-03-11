
namespace TheBoxSoftware.Documentation.Exporting
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Exports a Document using ExportSettings and an ExportConfigFile.
    /// </summary>
    /// <include file='../code-documentation/exporter.xml' path='docs/exporter[@name="class"]'/>
    public abstract class Exporter
    {
        private readonly IFileSystem _fileSystem;
        
        protected readonly int XmlExportStep = 10;

        private ExportCalculatedEventHandler _exportCalculated;
        private ExportStepEventHandler _exportStep;
        private ExportExceptionHandler _exportException;
        private ExportFailedEventHandler _exportFailure;
        private string _baseTempDirectory;   // base working directory for output and temp
        private Document _document;
        private string _applicationDirectory;
        private string _tempDirectory;
        private string _outputDirectory;
        private string _publishDirectory;
        private ExportSettings _settings;
        private ExportConfigFile _config;
        private System.Text.RegularExpressions.Regex _illegalFileCharacters;
        private int _currentExportStep;
        private bool _isCancelled;
        private List<Exception> _exportExceptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Exporter"/> class.
        /// </summary>
        protected Exporter(Document document, ExportSettings settings, ExportConfigFile config, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;

            _config = config;
            _settings = settings;
            _document = document;
            _exportExceptions = new List<Exception>();

            string regex = string.Format("{0}{1}",
                 new string(Path.GetInvalidFileNameChars()),
                 new string(Path.GetInvalidPathChars()));
            _illegalFileCharacters = new System.Text.RegularExpressions.Regex(
                string.Format("[{0}]", System.Text.RegularExpressions.Regex.Escape(regex))
                );
        }

        /// <summary>
        /// Factory method for creating new Exporter instances.
        /// </summary>
        /// <include file='../code-documentation/exporter.xml' path='docs/exporter[@name="Create"]'/>
        public static Exporter Create(Document document, ExportSettings settings, ExportConfigFile config)
        {
            if (document == null) throw new ArgumentNullException("document");
            if (settings == null) throw new ArgumentNullException("settings");
            if (config == null) throw new ArgumentNullException("config");

            Exporter createdExporter = null;

            switch (config.Exporter)
            {
                case Exporters.Html1:
                    createdExporter = new HtmlHelp1Exporter(document, settings, config);
                    break;

                case Exporters.Html2:
                    createdExporter = new HtmlHelp2Exporter(document, settings, config);
                    break;

                case Exporters.HelpViewer1:
                    createdExporter = new HelpViewer1Exporter(document, settings, config);
                    break;

                case Exporters.XML:
                    createdExporter = new XmlExporter(document, settings, config);
                    break;

                case Exporters.Website:
                default:
                    createdExporter = new WebsiteExporter(document, settings, config, new FileSystem());
                    break;
            }

            return createdExporter;
        }

        /// <summary>
        /// Performs the export steps necessary to produce the final exported documentation in the
        /// implementing type.
        /// </summary>
        public abstract void Export();

        /// <summary>
        /// When implemented in a dervied class checks to see if there are any issues with running
        /// the exporter.
        /// </summary>
        /// <returns>Should return a list of issues.</returns>
        public abstract List<Issue> GetIssues();

        /// <summary>
        /// Cancels the current export and cleans up.
        /// </summary>
        public virtual void Cancel()
        {
            IsCancelled = true;
        }

        /// <summary>
        /// A method that recursively, through the documentation tree, exports all of the
        /// found pages for each of the entries in the documentation.
        /// </summary>
        /// <param name="currentEntry">The current entry to export</param>
        protected virtual void RecursiveEntryExport(Entry currentEntry)
        {
            Export(currentEntry);
            for (int i = 0; i < currentEntry.Children.Count; i++)
            {
                RecursiveEntryExport(currentEntry.Children[i]);
            }
        }

        /// <summary>
        /// Exports the <paramref name="current"/> entry to intermediate XML format.
        /// </summary>
        /// <include file='../code-documentation/exporter.xml' path='docs/exporter[@name="export"]'/>
        protected virtual string Export(Entry current)
        {
            string filename = string.Format("{0}{1}{2}.xml",
                this.TempDirectory,
                current.Key,
                string.IsNullOrEmpty(current.SubKey) ? string.Empty : "-" + this.IllegalFileCharacters.Replace(current.SubKey, string.Empty)
                );

            try
            {
                Rendering.XmlRenderer r = Rendering.XmlRenderer.Create(current, Document);

                if (null == r)
                    ExportExceptions.Add(new Exception($"No XML renderer for the Entry {current.Name}"));

                using (System.Xml.XmlWriter writer = XmlWriter.Create(filename))
                {
                    r.Render(writer);
                }
            }
            catch (Exception ex)
            {
                if(_fileSystem.FileExists(filename))
                {
                    _fileSystem.DeleteFile(filename);
                }

                // ignore it and add it to the list of exceptions, try and add more details
                if (current != null)
                {
                    ExportException issue = new ExportException(
                        $"Failed to export member '{current.Name}'.",
                        ex);
                    ex = issue;
                }

                ExportExceptions.Add(ex);
            }

            return filename;
        }

        /// <summary>
        /// In some exporters generic types using angle brackets cause problems. This method creates
        /// a safe version of the name provided.
        /// </summary>
        /// <param name="name">The name to convert.</param>
        /// <returns>A safe version of <paramref name="name"/>.</returns>
        /// <remarks>
        /// A generic type GenericClass&lt;T&gt; will be output as GenericClass(T).
        /// </remarks>
        public static string CreateSafeName(string name)
        {
            return name.Replace('<', '(').Replace('>', ')');
        }

        /// <summary>
        /// Performs all the tasks necessary before starting the export process.
        /// </summary>
        protected void PrepareForExport()
        {
            // create the working directory
            string tempFileName = Path.GetTempFileName();
            _baseTempDirectory = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(tempFileName));
            _fileSystem.DeleteFile(tempFileName);

            TempDirectory = Path.Combine(_baseTempDirectory, "XML\\");
            _fileSystem.CreateDirectory(TempDirectory);
            OutputDirectory = Path.Combine(_baseTempDirectory, "Output\\");
            _fileSystem.CreateDirectory(OutputDirectory);

            // get the publish path and clean/create the directory
            if (string.IsNullOrEmpty(this.Settings.PublishDirectory))
            {
                // no output directory set, default to my documents live document folder
                string publishPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Live Documenter\\Published\\");
                this.PublishDirectory = publishPath;
            }
            else
            {
                this.PublishDirectory = Settings.PublishDirectory;
            }

            // #38 always add a new export directory to the publish location, this is to stop people deleting their
            // files and folders.
            DateTime now = DateTime.Now;
            if (_settings.OverwritePublishDirectory) {
                this.PublishDirectory = Path.Combine(PublishDirectory, "LD Export\\");
            }
            else {
                this.PublishDirectory = Path.Combine(PublishDirectory, string.Format("LD Export - {4:0000}{3:00}{2:00} {1:00}{0:00}\\", now.Minute, now.Hour, now.Day, now.Month, now.Year));
            }

            if (_fileSystem.DirectoryExists(PublishDirectory))
            {
                _fileSystem.DeleteDirectory(PublishDirectory, true);
                System.Threading.Thread.Sleep(0);
            }

            // #183 fixes issue as directory is not recreated when user has folder open in explorer
            int counter = 0;
            while (counter < 10 && _fileSystem.DirectoryExists(PublishDirectory))
            {
                counter++;
                System.Threading.Thread.Sleep(60);
            }

            _fileSystem.CreateDirectory(PublishDirectory);

            // read the current application directory
            this.ApplicationDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// Cleans up any working directories and files from the export operation.
        /// </summary>
        protected void Cleanup()
        {
            _fileSystem.DeleteDirectory(_baseTempDirectory, true);
        }
        
        /// <summary>
        /// Occurs when a step has been performed during the export operation.
        /// </summary>
        public event ExportStepEventHandler ExportStep
        {
            add
            {
                _exportStep += value;
            }
            remove
            {
                _exportStep -= value;
            }
        }

        /// <summary>
        /// Raises the <see cref="ExportStep" /> event.
        /// </summary>
        /// <param name="e">The <see cref="TheBoxSoftware.Documentation.Exporting.ExportStepEventArgs"/> instance containing the event data.</param>
        protected void OnExportStep(ExportStepEventArgs e)
        {
            _exportStep?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when the number of steps in the export operation has been calculated.
        /// </summary>
        public event ExportCalculatedEventHandler ExportCalculated
        {
            add => _exportCalculated += value;
            remove => _exportCalculated -= value;
        }

        /// <summary>
        /// Raises the <see cref="ExportCalculated"/> event.
        /// </summary>
        /// <param name="e">The <see cref="TheBoxSoftware.Documentation.Exporting.ExportCalculatedEventArgs"/> instance containing the event data.</param>
        protected void OnExportCalculated(ExportCalculatedEventArgs e)
        {
            _exportCalculated?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when an exception occurs in the export process.
        /// </summary>
        public event ExportExceptionHandler ExportException
        {
            add => _exportException += value;
            remove => _exportException -= value;
        }

        /// <summary>
        /// Raises the <see cref="ExportException"/> event.
        /// </summary>
        /// <param name="e">The <see cref="TheBoxSoftware.Documentation.Exporting.ExportExceptionEventArgs"/> instance containing the event data.</param>
        protected void OnExportException(ExportExceptionEventArgs e)
        {
            _exportException?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when an expected non-terminal failure occurs during an export run.
        /// </summary>
        public event ExportFailedEventHandler ExportFailed
        {
            add => _exportFailure += value;
            remove => _exportFailure -= value;
        }

        /// <summary>
        /// Raises the <see cref="ExportFailure"/> event.
        /// </summary>
        /// <param name="e">The details of the failure.</param>
        protected void OnExportFailed(ExportFailedEventArgs e)
        {
            _exportFailure?.Invoke(e);
        }

        /// <summary>
        /// The Document being exported.
        /// </summary>
        public Document Document
        {
            get => _document;
            set => _document = value;
        }

        /// <summary>
        /// The directory the application is executing from and installed.
        /// </summary>
        protected string ApplicationDirectory
        {
            get => _applicationDirectory;
            private set => _applicationDirectory = value;
        }

        /// <summary>
        /// The directory used to output the first rendered output to, this is not the output or publishing directory.
        /// </summary>
        protected string TempDirectory
        {
            get => _tempDirectory;
            private set => _tempDirectory = value;
        }

        /// <summary>
        /// The first staging folder where all the files come together to be completed or compiled.
        /// </summary>
        protected string OutputDirectory
        {
            get => _outputDirectory;
            private set => _outputDirectory = value;
        }

        /// <summary>
        /// Area to copy the final output to.
        /// </summary>
        protected string PublishDirectory
        {
            get => _publishDirectory;
            set => _publishDirectory = value;
        }

        /// <summary>
        /// The regular expression to check for illegal file characters before creating files.
        /// </summary>
        protected System.Text.RegularExpressions.Regex IllegalFileCharacters
        {
            get => _illegalFileCharacters;
            private set => _illegalFileCharacters = value;
        }

        /// <summary>
        /// The export settings.
        /// </summary>
        protected ExportSettings Settings
        {
            get => _settings;
            set => _settings = value;
        }

        /// <summary>
        /// The export configuration details.
        /// </summary>
        protected ExportConfigFile Config
        {
            get => _config;
            set => _config = value;
        }

        /// <summary>
        /// Counter indicating the current export step in the export process.
        /// </summary>
        protected int CurrentExportStep
        {
            get => _currentExportStep;
            set => _currentExportStep = value;
        }

        /// <summary>
        /// Indicates if this export has been cancelled.
        /// </summary>
        protected bool IsCancelled
        {
            get => _isCancelled;
            private set => _isCancelled = value;
        }

        /// <summary>
        /// A collection of errors that have occurred during the export process.
        /// </summary>
        public List<Exception> ExportExceptions
        {
            get => _exportExceptions;
            set => _exportExceptions = value;
        }
    }
}