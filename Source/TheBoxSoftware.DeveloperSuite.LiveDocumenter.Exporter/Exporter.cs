using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TheBoxSoftware.Documentation;
using export = TheBoxSoftware.Documentation.Exporting;
using TheBoxSoftware.Reflection;

namespace TheBoxSoftware.Exporter {
    /// <summary>
    /// Exporter class, reads the configuration information and exports the contents as requested.
    /// </summary>
    /// <remarks>
    /// <para>Verbose will slow down the processing time for the export but will provide more information
    /// about how the export is progressing.</para>
    /// </remarks>
	internal sealed class Exporter {
		private Configuration configuration;
        private string lastStep = string.Empty; // stores the last export step so we can work out where we are
        private bool verbose = false;           // indicates if the output information should be verbose or not

		/// <summary>
		/// Initialises a new instance of the Exporter
		/// </summary>
		/// <param name="configuration">The export configuration information.</param>
        /// <param name="verbose">Indicates if the output should be complete or limited.</param>
		public Exporter(Configuration configuration, bool verbose) {
			this.configuration = configuration;
            this.verbose = verbose;
		}

        /// <summary>
        /// Performs the export, exporting to the libraries to the outputs specified in the XML configuration
        /// file.
        /// </summary>
		public void Export() {
			List<DocumentedAssembly> files = new List<DocumentedAssembly>();
			Project project = null;
			export.ExportSettings settings = new export.ExportSettings();
			settings.Settings = new DocumentSettings();
            settings.Settings.VisibilityFilters = this.configuration.Filters;

			// initialise the assemblies to be documented
			if(Path.GetExtension(this.configuration.Document) == ".ldproj") {
                try
                {
                    project = Project.Deserialize(this.configuration.Document);
                }
                catch (InvalidOperationException e)
                {
                    Logger.Log(
                        string.Format("Invalid document '{0}' please fix the error and try again.\n  {1}", this.configuration.Document, e.Message),
                        LogType.Error
                        );
                    return; // bail we have an invalid ldproj file
                }
                finally { }
                files.AddRange(project.GetAssemblies());

                // override the filters if they are specified in the project
                if (!(settings.Settings.VisibilityFilters != null && settings.Settings.VisibilityFilters.Count > 0))
                {
                    settings.Settings.VisibilityFilters = project.VisibilityFilters;
                }
			}
			else if(Path.GetExtension(this.configuration.Document) == ".dll") {
				files.Add(new DocumentedAssembly(this.configuration.Document));
			}
			else {
				files.AddRange(
					InputFileReader.Read(
					this.configuration.Document, 
					"Release"
					));
			}

			// use the configurations visibility filters or default to just public
            if (settings.Settings.VisibilityFilters == null || settings.Settings.VisibilityFilters.Count == 0)
            {
                Logger.Log("No visibility filters are found defaulting to Public and Protected.\n", LogType.Warning);
                settings.Settings.VisibilityFilters = new List<Visibility>() { Visibility.Public };
            }
            else
            {
                List<string> filters = new List<string>();
                foreach (Visibility current in settings.Settings.VisibilityFilters)
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

            Logger.Verbose(string.Format("  {0} contains {1} members and types.\n", Path.GetFileName(configuration.Document), entryCreator.Created));

			// export the document in all the required formats
			foreach (Configuration.Output output in this.configuration.Outputs) {
				DateTime start = DateTime.Now;
				DateTime end;
				export.ExportConfigFile config = export.ExportConfigFile.Create(
					Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
					+ "\\ApplicationData\\" 
					+ output.File
					);

                Logger.Log(string.Format("\nExporting with {0} to location {1}.\n", output.File, output.Location), LogType.Progress);

				if (!config.IsValid) {
					Logger.Log(string.Format("There are issues with the LDEC file: {0}\n", output.File), LogType.Error);
				}
				else {
					settings.PublishDirectory = output.Location;

					export.Exporter exporter = export.Exporter.Create(d, settings, config);
                    if(this.verbose) exporter.ExportStep += new export.ExportStepEventHandler(exporter_ExportStep);
                    exporter.ExportException += new export.ExportExceptionHandler(exporter_ExportException);
                    if(this.verbose) exporter.ExportCalculated += new export.ExportCalculatedEventHandler(exporter_ExportCalculated);
                    exporter.ExportFailed += new export.ExportFailedEventHandler(exporter_ExportFailed);

					List<export.Issue> issues = exporter.GetIssues();
					if (issues.Count > 0) {
						foreach (export.Issue issue in issues) {
							Logger.Log(string.Format("{0}\n", issue.Description), LogType.Error);
						}
					}
					else {
						Logger.Verbose(string.Format("The export began at {0}.\n", start));
						exporter.Export();
						end = DateTime.Now;

						if (exporter.ExportExceptions != null && exporter.ExportExceptions.Count > 0) {
							Logger.Log("The export completed with the following issues:\n", LogType.Warning);
							foreach (Exception current in exporter.ExportExceptions) {
								Logger.Log(this.FormatExceptionData(current), LogType.Warning);
							}
						}
						
						Logger.Verbose(string.Format("The export completed at {0}, taking {1} seconds.\n", end, end.Subtract(start).TotalSeconds.ToString()));
					}
				}
			}
		}

		private void exporter_ExportStep(object sender, export.ExportStepEventArgs e) {
            if (this.lastStep == e.Description)
                return;
            else
            {
                this.lastStep = e.Description;
                Logger.Verbose(string.Format("  {0}\n", e.Description));
            }
		}

        private void exporter_ExportException(object sender, export.ExportExceptionEventArgs e)
        {
            Logger.Log(string.Format("{0}\n", e.Exception.Message), LogType.Error);
        }

        private void exporter_ExportCalculated(object sender, export.ExportCalculatedEventArgs e)
        {
            Logger.Verbose("Export started\n");
        }

        private void exporter_ExportFailed(export.ExportFailedEventArgs e)
        {
            Logger.Log(string.Format("{0}\n", e.Message), LogType.Error);
        }

		private string FormatExceptionData(Exception forException) {
			StringBuilder sb = new StringBuilder();
			if (forException != null) {
				sb.AppendLine(string.Format("Message: {0}", forException.Message));
				if (forException is IExtendedException) {
					sb.Append(((IExtendedException)forException).GetExtendedInformation());
					sb.AppendLine();
				}
				sb.AppendLine(forException.StackTrace);
			}

			return sb.ToString();
		}
	}
}
