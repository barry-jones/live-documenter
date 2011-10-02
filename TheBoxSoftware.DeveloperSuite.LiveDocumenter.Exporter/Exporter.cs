using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TheBoxSoftware.Documentation;
using export = TheBoxSoftware.Documentation.Exporting;
using TheBoxSoftware.Reflection;

namespace TheBoxSoftware.Exporter {
	internal sealed class Exporter {
		private Configuration configuration;

		/// <summary>
		/// Initialises a new instance of the Exporter
		/// </summary>
		/// <param name="configuration"></param>
		public Exporter(Configuration configuration) {
			this.configuration = configuration;
		}

		public void Export() {
			System.Diagnostics.Debugger.Launch();

			List<DocumentedAssembly> files = new List<DocumentedAssembly>();
			Project project = null;
			export.ExportSettings settings = new export.ExportSettings();
			settings.Settings = new DocumentSettings();

			// initialise the assemblies to be documented
			if(Path.GetExtension(this.configuration.Document) == ".ldproj") {
				project = Project.Deserialize(this.configuration.Document);
				foreach (string file in project.Files) {
					files.Add(new DocumentedAssembly(file));
				}
				settings.Settings.VisibilityFilters = project.VisibilityFilters;
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
			if (this.configuration.Filters != null && this.configuration.Filters.Count > 0) {
			}
			else if (settings.Settings.VisibilityFilters == null || settings.Settings.VisibilityFilters.Count == 0) {
				settings.Settings.VisibilityFilters = new List<Visibility>() { Visibility.Public };
			}

			// initialise the document
			EntryCreator entryCreator = new EntryCreator();
			Documentation.Document d = new Documentation.Document(files, Mappers.GroupedNamespaceFirst, false, entryCreator);
			d.Settings = settings.Settings;
			d.UpdateDocumentMap();

			Console.WriteLine("Exporting {0}", configuration.Document);
			Console.WriteLine("  containing {0} members and types.", entryCreator.Created);

			// export the document in all the required formats
			foreach (Configuration.Output output in this.configuration.Outputs) {
				DateTime start = DateTime.Now;
				DateTime end;
				export.ExportConfigFile config = export.ExportConfigFile.Create(
					Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
					+ "\\ApplicationData\\" 
					+ output.File
					);

				Console.WriteLine("Exporting with {0} to location {1}.", output.File, output.Location);

				if (!config.IsValid) {
					Console.WriteLine(" There are issues with the LDEC file: {0}", output.File);
				}
				else {
					settings.PublishDirectory = output.Location;

					export.Exporter exporter = export.Exporter.Create(d, settings, config);
					List<export.Issue> issues = exporter.GetIssues();
					if (issues.Count > 0) {
						Console.WriteLine("  The following issues occurred with the {0} exporter.", output.File);
						foreach (export.Issue issue in issues) {
							Console.WriteLine("  {0}", issue.Description);
						}
					}
					else {
						Console.WriteLine("The export began at {0}.", start);
						exporter.Export();
						end = DateTime.Now;

						if (exporter.ExportExceptions != null && exporter.ExportExceptions.Count > 0) {
							Console.WriteLine("The export completed with the following issues...");
							foreach (Exception current in exporter.ExportExceptions) {
								Console.WriteLine(this.FormatExceptionData(current));
							}
						}
						else {
							Console.WriteLine("The export completed at {0}, taking {1} minutes.", end, end.Subtract(start).TotalMinutes.ToString());
						}
					}
				}
			}
		}

		void exporter_ExportStep(object sender, export.ExportStepEventArgs e) {
			Console.WriteLine("  {0}", e.Description);
		}

		private string FormatExceptionData(Exception forException) {
			StringBuilder sb = new StringBuilder();
			if (forException != null) {
				sb.AppendLine();
				sb.AppendLine("----------------------------------------------------------");
				sb.AppendLine(string.Format("Message: {0}", forException.Message));
				sb.AppendLine();
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
