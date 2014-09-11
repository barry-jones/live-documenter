using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace TheBoxSoftware.Documentation.Exporting {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;

	/// <summary>
	/// Exports a Document using ExportSettings and an ExportConfigFile.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class will not throw exceptions in the Export method. All exceptions will be driven
	/// through the <see cref="ExportException"/> event.
	/// </para>
	/// <para>
	/// Implementers of derived classes should make sure that this export exception mechanism
	/// is continued. As this method is likely to be called on seperate threads.
	/// </para>
	/// </remarks>
	public abstract class Exporter {
        // 68 bytes
		private ExportCalculatedEventHandler exportCalculated;
		private ExportStepEventHandler exportStep;
		private ExportExceptionHandler exportException;
		private ExportFailedEventHandler exportFailure;
		private string baseTempDirectory;	// base working directory for output and temp
		protected readonly int XmlExportStep = 10;
        private Document document;
        private string applicationDirectory;
        private string tempDirectory;
        private string outputDirectory;
        private string publishDirectory;
        private ExportSettings settings;
        private ExportConfigFile config;
        private System.Text.RegularExpressions.Regex illegalFileCharecters;
        private int currentExportStep;
        private bool isCancelled;
        private List<Exception> exportExceptions;


		/// <summary>
		/// Initializes a new instance of the <see cref="Exporter"/> class.
		/// </summary>
		/// <param name="currentFiles">The current files.</param>
		protected Exporter(Document document, ExportSettings settings, ExportConfigFile config) {
			this.Config = config;
			this.Settings = settings;
			this.Document = document;
			this.ExportExceptions = new List<Exception>();

			string regex = string.Format("{0}{1}",
				 new string(Path.GetInvalidFileNameChars()),
				 new string(Path.GetInvalidPathChars()));
			this.IllegalFileCharacters = new System.Text.RegularExpressions.Regex(
				string.Format("[{0}]", System.Text.RegularExpressions.Regex.Escape(regex))
				);
		}

		/// <summary>
		/// Factory method for creating new Exporter instances.
		/// </summary>
		/// <param name="document">The document to export.</param>
		/// <param name="config">The export configuration.</param>
		/// <returns>A valid instance of an Exporter.</returns>
		/// <exception cref="ArgumentNullException">
		/// All of the parameters are required so provided a null reference will cause this exception
		/// please see the parameter name in the exception for more information.
		/// </exception>
		public static Exporter Create(Document document, ExportSettings settings, ExportConfigFile config) {
			if (document == null) throw new ArgumentNullException("document");
			if (settings == null) throw new ArgumentNullException("settings");
			if (config == null) throw new ArgumentNullException("config");

			Exporter createdExporter = null;

			switch (config.Exporter) {
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
					createdExporter = new WebsiteExporter(document, settings, config);
					break;
			}

			return createdExporter;
		}

		#region Properties
		/// <summary>
		/// The Document being exported.
		/// </summary>
		public Document Document {
            get { return this.document; }
            set { this.document = value; }
        }

		/// <summary>
		/// The directory the application is executing from and installed.
		/// </summary>
		protected string ApplicationDirectory {
            get { return this.applicationDirectory; }
            private set { this.applicationDirectory = value; }
        }

		/// <summary>
		/// The directory used to output the first rendered output to, this is not the output or publishing directory.
		/// </summary>
		protected string TempDirectory {
            get { return this.tempDirectory; }
            private set { this.tempDirectory = value; }
        }

		/// <summary>
		/// The first staging folder where all the files come together to be completed or compiled.
		/// </summary>
		protected string OutputDirectory {
            get { return this.outputDirectory; }
            private set { this.outputDirectory = value; }
        }

		/// <summary>
		/// Area to copy the final output to.
		/// </summary>
		protected string PublishDirectory {
            get { return this.publishDirectory; }
            set { this.publishDirectory = value; }
        }

		/// <summary>
		/// The regular expression to check for illegal file characters before creating files.
		/// </summary>
		protected System.Text.RegularExpressions.Regex IllegalFileCharacters {
            get { return this.illegalFileCharecters; }
            private set { this.illegalFileCharecters = value; }
        }

		/// <summary>
		/// The export settings.
		/// </summary>
		protected ExportSettings Settings {
            get { return this.settings; }
            set { this.settings = value; }
        }

		/// <summary>
		/// The export configuration details.
		/// </summary>
		protected ExportConfigFile Config {
            get { return this.config; }
            set { this.config = value; }
        }

		/// <summary>
		/// Counter indicating the current export step in the export process.
		/// </summary>
		protected int CurrentExportStep {
            get { return this.currentExportStep; }
            set { this.currentExportStep = value; }
        }

		/// <summary>
		/// Indicates if this export has been cancelled.
		/// </summary>
		protected bool IsCancelled {
            get { return this.isCancelled; }
            private set { this.isCancelled = value; }
        }

		/// <summary>
		/// A collection of errors that have occurred during the export process.
		/// </summary>
		public List<Exception> ExportExceptions {
            get { return this.exportExceptions; }
            set { this.exportExceptions = value; }
        }
		#endregion

		/// <summary>
		/// Performs the export steps necessary to produce the final exported documentation in the
		/// implementing type.
		/// </summary>
		public abstract void Export();

		public abstract Stream ExportMember(Entry entry);

		/// <summary>
		/// When implemented in a dervied class checks to see if there are any issues with running
		/// the exporter.
		/// </summary>
		/// <returns>Should return a list of issues.</returns>
		public abstract List<Issue> GetIssues();

		/// <summary>
		/// Cancels the current export and cleans up.
		/// </summary>
		public virtual void Cancel() {
			this.IsCancelled = true;
		}

		/// <summary>
		/// A method that recursively, through the documentation tree, exports all of the
		/// found pages for each of the entries in the documentation.
		/// </summary>
		/// <param name="currentEntry">The current entry to export</param>
		protected virtual void RecursiveEntryExport(Entry currentEntry) {
			this.Export(currentEntry);
			for (int i = 0; i < currentEntry.Children.Count; i++) {
				this.RecursiveEntryExport(currentEntry.Children[i]);
			}
		}

		/// <summary>
		/// Exports the current entry.
		/// </summary>
		/// <param name="current">The current entry to export.</param>
		/// <returns>The name of the rendered XML file</returns>
		protected virtual string Export(Entry current) {
			string filename = string.Format("{0}{1}{2}.xml",
				this.TempDirectory,
				current.Key,
				string.IsNullOrEmpty(current.SubKey) ? string.Empty : "-" + this.IllegalFileCharacters.Replace(current.SubKey, string.Empty)
				);

			try {
				Rendering.XmlRenderer r = Rendering.XmlRenderer.Create(current, this.Document);
				if (r != null) {
					using (System.Xml.XmlWriter writer = XmlWriter.Create(filename)) {
						r.Render(writer);
					}
				}
			}
			catch (Exception ex) {
				if(System.IO.File.Exists(filename)) { 
				    System.IO.File.Delete(filename);
				}
				
                // we will deal with it later
                if(current != null)
                {
                    ExportException issue = new ExportException(
                        string.Format("Failed to export member '{0}'.", current.Name),
                        ex);
                    ex = issue;
                }

				this.ExportExceptions.Add(ex);
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
		public static string CreateSafeName(string name) {
			return name.Replace('<', '(').Replace('>', ')');
		}

		/// <summary>
		/// Performs all the tasks necessary before starting the export process.
		/// </summary>
		protected void PrepareForExport() {
			// create the working directory
			string tempFileName = Path.GetTempFileName();
			this.baseTempDirectory = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(tempFileName));
			File.Delete(tempFileName);	// dont leave zero byte files all over the place.

			this.TempDirectory = Path.Combine(this.baseTempDirectory, "XML\\");
			Directory.CreateDirectory(this.TempDirectory);
			this.OutputDirectory = Path.Combine(this.baseTempDirectory, "Output\\");
			Directory.CreateDirectory(this.OutputDirectory);

			// get the publish path and clean/create the directory
			if (string.IsNullOrEmpty(this.Settings.PublishDirectory)) {
				// no output directory set, default to my documents live document folder
				string publishPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Live Documenter\\Published\\");
				this.PublishDirectory = publishPath;
			}
			else {
				this.PublishDirectory = this.Settings.PublishDirectory;
			}

			if (Directory.Exists(this.PublishDirectory)) {
				Directory.Delete(this.PublishDirectory, true);
				System.Threading.Thread.Sleep(0);
			}

			// #183 fixes issue as directory is not recreated when user has folder open in explorer
			int counter = 0;
			while (counter < 10 && Directory.Exists(this.PublishDirectory)) {
				counter++;
				System.Threading.Thread.Sleep(60);
			}

			Directory.CreateDirectory(this.PublishDirectory);
			
			// read the current application directory
			this.ApplicationDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}

		/// <summary>
		/// Cleans up any working directories and files from the export operation.
		/// </summary>
		protected void Cleanup() {
			Directory.Delete(this.baseTempDirectory, true);
		}

		#region Events
		/// <summary>
		/// Occurs when a step has been performed during the export operation.
		/// </summary>
		public event ExportStepEventHandler ExportStep {
			add { this.exportStep += value; }
			remove { this.exportStep -= value; }
		}

		/// <summary>
		/// Raises the <see cref="ExportStep" /> event.
		/// </summary>
		/// <param name="e">The <see cref="TheBoxSoftware.Documentation.Exporting.ExportStepEventArgs"/> instance containing the event data.</param>
		protected void OnExportStep(ExportStepEventArgs e) {
			if (this.exportStep != null) {
				this.exportStep(this, e);
			}
		}

		/// <summary>
		/// Occurs when the number of steps in the export operation has been calculated.
		/// </summary>
		public event ExportCalculatedEventHandler ExportCalculated {
			add { this.exportCalculated += value; }
			remove { this.exportCalculated -= value; }
		}

		/// <summary>
		/// Raises the <see cref="ExportCalculated"/> event.
		/// </summary>
		/// <param name="e">The <see cref="TheBoxSoftware.Documentation.Exporting.ExportCalculatedEventArgs"/> instance containing the event data.</param>
		protected void OnExportCalculated(ExportCalculatedEventArgs e) {
			if (this.exportCalculated != null) {
				this.exportCalculated(this, e);
			}
		}

		/// <summary>
		/// Occurs when an exception occurs in the export process.
		/// </summary>
		public event ExportExceptionHandler ExportException {
			add { this.exportException += value; }
			remove { this.exportException -= value; }
		}

		/// <summary>
		/// Raises the <see cref="ExportException"/> event.
		/// </summary>
		/// <param name="e">The <see cref="TheBoxSoftware.Documentation.Exporting.ExportExceptionEventArgs"/> instance containing the event data.</param>
		protected void OnExportException(ExportExceptionEventArgs e) {
			if (this.exportException != null) {
				this.exportException(this, e);
			}
		}

		/// <summary>
		/// Occurs when an expected non-terminal failure occurs during an export run.
		/// </summary>
		public event ExportFailedEventHandler ExportFailed {
			add { this.exportFailure += value; }
			remove { this.exportFailure -= value; }
		}

		/// <summary>
		/// Raises the <see cref="ExportFailure"/> event.
		/// </summary>
		/// <param name="e">The details of the failure.</param>
		protected void OnExportFailed(ExportFailedEventArgs e) {
			if (this.exportFailure != null) {
				this.exportFailure(e);
			}
		}
		#endregion
	}
}
