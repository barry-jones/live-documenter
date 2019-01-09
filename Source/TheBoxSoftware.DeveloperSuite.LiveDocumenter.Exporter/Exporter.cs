
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

            Document document = InitialiseDocumentForExport(files, settings);

            foreach(Configuration.Output output in _configuration.Outputs)
            {
                ExportToOutputMethod(settings, document, output);
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
            EntryCreator entryCreator = new EntryCreator();
            Document document = new Document(files, Mappers.GroupedNamespaceFirst, false, entryCreator);

            document.Settings = settings.Settings;
            document.UpdateDocumentMap();

            _log.LogInformation($"  {Path.GetFileName(_configuration.Document)} contains {entryCreator.Created} members and types.\n");

            return document;
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