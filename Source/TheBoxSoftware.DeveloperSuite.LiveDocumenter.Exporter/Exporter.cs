
namespace TheBoxSoftware.Exporter
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using TheBoxSoftware.Documentation;
    using export = TheBoxSoftware.Documentation.Exporting;
    using TheBoxSoftware.Reflection;

    /// <summary>
    /// Exporter class, reads the configuration information and exports the contents as requested.
    /// </summary>
    /// <remarks>
    /// <para>Verbose will slow down the processing time for the export but will provide more information
    /// about how the export is progressing.</para>
    /// </remarks>
	internal sealed class Exporter
    {
        private readonly ILog _log;
        private Configuration _configuration;
        private string _lastStep = string.Empty; // stores the last export step so we can work out where we are
        private bool _verbose = false;           // indicates if the output information should be verbose or not
        private Dictionary<Visibility, List<string>> _skippedMembers;  // collects members filtered by visibility
        private List<Visibility> _visibilityFilters;  // visibility filters to report on
        private bool _dryRun = false;            // indicates if this is a dry-run validation

        /// <summary>
        /// Initialises a new instance of the Exporter
        /// </summary>
        /// <param name="configuration">The export configuration information.</param>
        /// <param name="verbose">Indicates if the output should be complete or limited.</param>
        /// <param name="log">The ILog instance to write export details to.</param>
        public Exporter(Configuration configuration, bool verbose, ILog log)
        {
            _log = log;
            _configuration = configuration;
            _verbose = verbose;
        }

        /// <summary>
        /// Performs the export, exporting to the libraries to the outputs specified in the XML configuration
        /// file.
        /// </summary>
		public void Export()
        {
            _dryRun = false;
            List<DocumentedAssembly> files = new List<DocumentedAssembly>();
            Project project = null;
            export.ExportSettings settings = new export.ExportSettings();
            settings.Settings = new DocumentSettings();
            settings.Settings.VisibilityFilters = _configuration.Filters;

            // initialise the assemblies to be documented
            if(Path.GetExtension(_configuration.Document) == ".ldproj")
            {
                try
                {
                    project = Project.Deserialize(_configuration.Document);
                }
                catch(InvalidOperationException e)
                {
                    _log.LogError(
                        $"Invalid document '{_configuration.Document}' please fix the error and try again.\n  {e.Message}"
                        );
                    return; // bail we have an invalid ldproj file
                }
                finally { }
                files.AddRange(project.GetAssemblies());

                // override the filters if they are specified in the project
                if(!(settings.Settings.VisibilityFilters != null && settings.Settings.VisibilityFilters.Count > 0))
                {
                    settings.Settings.VisibilityFilters = project.VisibilityFilters;
                }
            }
            else if(Path.GetExtension(_configuration.Document) == ".dll")
            {
                files.Add(new DocumentedAssembly(_configuration.Document));
            }
            else
            {
                files.AddRange(
                    new InputFileReader().Read(
                    _configuration.Document,
                    "Release"
                    ));
            }

            // use the configurations visibility filters or default to just public
            if(settings.Settings.VisibilityFilters == null || settings.Settings.VisibilityFilters.Count == 0)
            {
                _log.LogWarning("No visibility filters are found defaulting to Public and Protected.\n");
                settings.Settings.VisibilityFilters = new List<Visibility>() { Visibility.Public };
            }
            else
            {
                List<string> filters = new List<string>();
                foreach(Visibility current in settings.Settings.VisibilityFilters)
                {
                    filters.Add(Enum.GetName(typeof(Visibility), current));
                }
                _log.LogInformation($"Details:\n  Visible members: ({string.Join("|", filters.ToArray())})\n");
            }

            try
            {
                Document document = InitialiseDocumentForExport(files, settings);

                foreach(Configuration.Output output in _configuration.Outputs)
                {
                    ExportToOutputMethod(settings, document, output);
                }

                if(_verbose)
                {
                    LogSkipSummary();
                }
            }
            catch(Exception ex)
            {
                _log.LogError($"Failed to initialize document for export: {ex.Message}\n");
            }
        }

        /// <summary>
        /// Validates the configuration and assemblies without writing output. Returns 0 on success, 1 on failure.
        /// </summary>
        public int ValidateAndLogSummary()
        {
            _dryRun = true;
            _log.LogInformation("Performing dry-run validation...\n");

            List<DocumentedAssembly> files = new List<DocumentedAssembly>();
            Project project = null;
            export.ExportSettings settings = new export.ExportSettings();
            settings.Settings = new DocumentSettings();
            settings.Settings.VisibilityFilters = _configuration.Filters;

            // initialise the assemblies to be documented
            if(Path.GetExtension(_configuration.Document) == ".ldproj")
            {
                try
                {
                    project = Project.Deserialize(_configuration.Document);
                }
                catch(InvalidOperationException e)
                {
                    _log.LogError(
                        $"Invalid document '{_configuration.Document}' please fix the error and try again.\n  {e.Message}"
                        );
                    return 1;
                }
                files.AddRange(project.GetAssemblies());

                if(!(settings.Settings.VisibilityFilters != null && settings.Settings.VisibilityFilters.Count > 0))
                {
                    settings.Settings.VisibilityFilters = project.VisibilityFilters;
                }
            }
            else if(Path.GetExtension(_configuration.Document) == ".dll")
            {
                files.Add(new DocumentedAssembly(_configuration.Document));
            }
            else
            {
                files.AddRange(
                    new InputFileReader().Read(
                    _configuration.Document,
                    "Release"
                    ));
            }

            if(settings.Settings.VisibilityFilters == null || settings.Settings.VisibilityFilters.Count == 0)
            {
                _log.LogWarning("No visibility filters are found defaulting to Public and Protected.\n");
                settings.Settings.VisibilityFilters = new List<Visibility>() { Visibility.Public };
            }
            else
            {
                List<string> filters = new List<string>();
                foreach(Visibility current in settings.Settings.VisibilityFilters)
                {
                    filters.Add(Enum.GetName(typeof(Visibility), current));
                }
                _log.LogInformation($"Details:\n  Visible members: ({string.Join("|", filters.ToArray())})\n");
            }

            try
            {
                Document document = InitialiseDocumentForExport(files, settings);

                // Validate all LDEC files without exporting
                foreach(Configuration.Output output in _configuration.Outputs)
                {
                    export.ExportConfigFile config = new export.ExportConfigFile(
                        Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
                        + "\\ApplicationData\\"
                        + output.File
                        );
                    config.Initialise();

                    _log.LogInformation($"Validating LDEC file {output.File} for export to {output.Location}.\n");

                    if(!config.IsValid)
                    {
                        _log.LogError($"There are issues with the LDEC file: {output.File}\n");
                        List<export.Issue> validationIssues = config.GetValidationIssues();
                        foreach(export.Issue issue in validationIssues)
                        {
                            _log.LogError($"  {issue.Description}\n");
                        }
                        return 1;
                    }
                }

                _log.LogInformation("Dry-run validation successful. All configurations and assemblies are valid.\n");

                if(_verbose)
                {
                    LogSkipDetails();
                }

                return 0;
            }
            catch(Exception ex)
            {
                _log.LogError($"Failed to validate: {ex.Message}\n");
                return 1;
            }
        }

        private void ExportToOutputMethod(export.ExportSettings settings, Document document, Configuration.Output output)
        {
            DateTime start = DateTime.Now;
            DateTime end;
            export.ExportConfigFile config = new export.ExportConfigFile(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
                + "\\ApplicationData\\"
                + output.File
                );
            config.Initialise();

            _log.LogProgress($"\nExporting with {output.File} to location {output.Location}.\n");

            if(!config.IsValid)
            {
                _log.LogError($"There are issues with the LDEC file: {output.File}\n");
                List<export.Issue> validationIssues = config.GetValidationIssues();
                foreach(export.Issue issue in validationIssues)
                {
                    _log.LogError($"  {issue.Description}\n");
                }
            }
            else
            {
                settings.PublishDirectory = output.Location;

                export.Exporter exporter = export.Exporter.Create(document, settings, config);
                exporter.ExportStep += new export.ExportStepEventHandler(exporter_ExportStep);
                exporter.ExportException += new export.ExportExceptionHandler(exporter_ExportException);
                exporter.ExportCalculated += new export.ExportCalculatedEventHandler(exporter_ExportCalculated);
                exporter.ExportFailed += new export.ExportFailedEventHandler(exporter_ExportFailed);

                List<export.Issue> issues = exporter.GetIssues();
                if(issues.Count > 0)
                {
                    foreach(export.Issue issue in issues)
                    {
                        _log.LogError($"{issue.Description}\n");
                    }
                }
                else
                {
                    _log.LogInformation($"The export began at {start}.\n");
                    exporter.Export();
                    end = DateTime.Now;

                    if(exporter.ExportExceptions != null && exporter.ExportExceptions.Count > 0)
                    {
                        _log.LogWarning("The export completed with the following issues:\n");
                        foreach(Exception current in exporter.ExportExceptions)
                        {
                            _log.LogWarning(FormatExceptionData(current));
                        }
                    }

                    _log.LogInformation($"The export completed at {end}, taking {end.Subtract(start).ToString()}.\n");
                }
            }
        }

        private Document InitialiseDocumentForExport(List<DocumentedAssembly> files, export.ExportSettings settings)
        {
            _skippedMembers = new Dictionary<Visibility, List<string>>();
            _visibilityFilters = settings.Settings.VisibilityFilters ?? new List<Visibility>();

            EntryCreator entryCreator = new EntryCreator();
            Document document = new Document(files, Mappers.GroupedNamespaceFirst, false, entryCreator);

            // Subscribe to skip-observer for filtering tracking
            if(_verbose)
            {
                document.Mapper.PreEntryAdded += OnPreEntryAdded;
            }

            document.Settings = settings.Settings;
            document.UpdateDocumentMap();

            _log.LogInformation($"  {Path.GetFileName(_configuration.Document)} contains {entryCreator.Created} members and types.\n");

            return document;
        }

        private void OnPreEntryAdded(object sender, PreEntryAddedEventArgs e)
        {
            if(e.Filter && e.Member != null)
            {
                Visibility visibility = e.Member.MemberAccess;
                if(!_skippedMembers.ContainsKey(visibility))
                {
                    _skippedMembers[visibility] = new List<string>();
                }
                _skippedMembers[visibility].Add(e.Member.Name);
            }
        }

        private void LogSkipSummary()
        {
            _log.LogInformation("[Visibility]\n");
            foreach(Visibility visibility in _visibilityFilters)
            {
                int count = _skippedMembers.ContainsKey(visibility) ? _skippedMembers[visibility].Count : 0;
                string visibilityName = Enum.GetName(typeof(Visibility), visibility);
                _log.LogInformation($"{visibilityName} — {count} members excluded.\n");
            }
        }

        private void LogSkipDetails()
        {
            foreach(Visibility visibility in _visibilityFilters)
            {
                if(_skippedMembers.ContainsKey(visibility) && _skippedMembers[visibility].Count > 0)
                {
                    string visibilityName = Enum.GetName(typeof(Visibility), visibility);
                    _log.LogInformation($"{visibilityName}:\n");
                    foreach(string memberName in _skippedMembers[visibility])
                    {
                        _log.LogInformation($"  {memberName}\n");
                    }
                }
            }
        }

        private void exporter_ExportStep(object sender, export.ExportStepEventArgs e)
        {
            if(_lastStep == e.Description)
                return;
            else
            {
                _lastStep = e.Description;
                _log.LogInformation($"  {e.Description}\n");
            }
        }

        private void exporter_ExportException(object sender, export.ExportExceptionEventArgs e)
        {
            _log.LogError($"{e.Exception.Message}\n");
        }

        private void exporter_ExportCalculated(object sender, export.ExportCalculatedEventArgs e)
        {
            _log.LogInformation("Export started\n");
        }

        private void exporter_ExportFailed(export.ExportFailedEventArgs e)
        {
            _log.LogError($"{e.Message}\n");
        }

        private string FormatExceptionData(Exception forException)
        {
            StringBuilder sb = new StringBuilder();
            if(forException != null)
            {
                sb.AppendLine($"Message: {forException.Message}");
                if(forException is IExtendedException)
                {
                    sb.Append(((IExtendedException)forException).GetExtendedInformation());
                    sb.AppendLine();
                }
                sb.AppendLine(forException.StackTrace);
            }

            return sb.ToString();
        }
    }
}