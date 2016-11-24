
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
        private Configuration _configuration;
        private string _lastStep = string.Empty; // stores the last export step so we can work out where we are
        private bool _verbose = false;           // indicates if the output information should be verbose or not

        /// <summary>
        /// Initialises a new instance of the Exporter
        /// </summary>
        /// <param name="configuration">The export configuration information.</param>
        /// <param name="verbose">Indicates if the output should be complete or limited.</param>
        public Exporter(Configuration configuration, bool verbose)
        {
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
                    Logger.Log(
                        $"Invalid document '{_configuration.Document}' please fix the error and try again.\n  {e.Message}",
                        LogType.Error
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
                    InputFileReader.Read(
                    _configuration.Document,
                    "Release"
                    ));
            }

            // use the configurations visibility filters or default to just public
            if(settings.Settings.VisibilityFilters == null || settings.Settings.VisibilityFilters.Count == 0)
            {
                Logger.Log("No visibility filters are found defaulting to Public and Protected.\n", LogType.Warning);
                settings.Settings.VisibilityFilters = new List<Visibility>() { Visibility.Public };
            }
            else
            {
                List<string> filters = new List<string>();
                foreach(Visibility current in settings.Settings.VisibilityFilters)
                {
                    filters.Add(Enum.GetName(typeof(Visibility), current));
                }
                Logger.Verbose(
                    string.Format("Details:\n  Visible members: ({0})\n", string.Join("|", filters.ToArray()))
                    );
            }

            // initialise the document
            EntryCreator entryCreator = new EntryCreator();
            Documentation.Document d = new Documentation.Document(files, Mappers.GroupedNamespaceFirst, false, entryCreator);
            d.Settings = settings.Settings;
            d.UpdateDocumentMap();

            Logger.Verbose($"  {Path.GetFileName(_configuration.Document)} contains {entryCreator.Created} members and types.\n");

            // export the document in all the required formats
            foreach(Configuration.Output output in _configuration.Outputs)
            {
                DateTime start = DateTime.Now;
                DateTime end;
                export.ExportConfigFile config = export.ExportConfigFile.Create(
                    Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
                    + "\\ApplicationData\\"
                    + output.File
                    );

                Logger.Log($"\nExporting with {output.File} to location {output.Location}.\n", LogType.Progress);

                if(!config.IsValid)
                {
                    Logger.Log(string.Format("There are issues with the LDEC file: {0}\n", output.File), LogType.Error);
                }
                else
                {
                    settings.PublishDirectory = output.Location;

                    export.Exporter exporter = export.Exporter.Create(d, settings, config);
                    if(_verbose) exporter.ExportStep += new export.ExportStepEventHandler(exporter_ExportStep);
                    exporter.ExportException += new export.ExportExceptionHandler(exporter_ExportException);
                    if(_verbose) exporter.ExportCalculated += new export.ExportCalculatedEventHandler(exporter_ExportCalculated);
                    exporter.ExportFailed += new export.ExportFailedEventHandler(exporter_ExportFailed);

                    List<export.Issue> issues = exporter.GetIssues();
                    if(issues.Count > 0)
                    {
                        foreach(export.Issue issue in issues)
                        {
                            Logger.Log($"{issue.Description}\n", LogType.Error);
                        }
                    }
                    else
                    {
                        Logger.Verbose($"The export began at {start}.\n");
                        exporter.Export();
                        end = DateTime.Now;

                        if(exporter.ExportExceptions != null && exporter.ExportExceptions.Count > 0)
                        {
                            Logger.Log("The export completed with the following issues:\n", LogType.Warning);
                            foreach(Exception current in exporter.ExportExceptions)
                            {
                                Logger.Log(FormatExceptionData(current), LogType.Warning);
                            }
                        }

                        Logger.Verbose($"The export completed at {end}, taking {end.Subtract(start).ToString()}.\n");
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
                Logger.Verbose($"  {e.Description}\n");
            }
        }

        private void exporter_ExportException(object sender, export.ExportExceptionEventArgs e)
        {
            Logger.Log($"{e.Exception.Message}\n", LogType.Error);
        }

        private void exporter_ExportCalculated(object sender, export.ExportCalculatedEventArgs e)
        {
            Logger.Verbose("Export started\n");
        }

        private void exporter_ExportFailed(export.ExportFailedEventArgs e)
        {
            Logger.Log($"{e.Message}\n", LogType.Error);
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