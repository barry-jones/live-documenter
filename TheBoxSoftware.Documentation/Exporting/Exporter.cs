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
	/// 
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
		private ExportCalculatedEventHandler exportCalculated;
		private ExportStepEventHandler exportStep;
		private ExportExceptionHandler exportException;
		private ExportFailedEventHandler exportFailure;

		/// <summary>
		/// Initializes a new instance of the <see cref="Exporter"/> class.
		/// </summary>
		/// <param name="currentFiles">The current files.</param>
		protected Exporter(List<DocumentedAssembly> currentFiles, ExportSettings settings, ExportConfigFile config) {
			this.CurrentFiles = currentFiles;
			this.Settings = settings;
			this.Config = config;

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
		/// <param name="files">The files to export documentation for.</param>
		/// <param name="settings">The export settings.</param>
		/// <param name="config">The export configuration.</param>
		/// <returns>A valid instance of an Exporter.</returns>
		/// <exception cref="ArgumentNullException">
		/// All of the parameters are required so provided a null reference will cause this exception
		/// please see the parameter name in the exception for more information.
		/// </exception>
		public static Exporter Create(List<DocumentedAssembly> files, ExportSettings settings, ExportConfigFile config) {
			if (files == null || files.Count == 0) throw new ArgumentNullException("files");
			if (settings == null) throw new ArgumentNullException("settings");
			if (config == null) throw new ArgumentNullException("config");

			Exporter createdExporter = null;

			switch (config.Exporter) {
				case Exporters.Html1:
					createdExporter = new HtmlHelp1Exporter(files, settings, config);
					break;

				case Exporters.Html2:
					createdExporter = new HtmlHelp2Exporter(files, settings, config);
					break;

				case Exporters.HelpViewer1:
					createdExporter = new HelpViewer1Exporter(files, settings, config);
					break;

				case Exporters.Website:
				default:
					createdExporter = new WebsiteExporter(files, settings, config);
					break;
			}

			return createdExporter;
		}

		#region Properties
		/// <summary>
		/// The document map to be exported.
		/// </summary>
		public DocumentMap DocumentMap { get; set; }

		/// <summary>
		///  The files that are to be documented.
		/// </summary>
		protected List<DocumentedAssembly> CurrentFiles { get; set; }

		/// <summary>
		/// The directory used to output the first rendered output to, this is not the output or publishing directory.
		/// </summary>
		protected string TempDirectory { get; set; }

		/// <summary>
		/// The first staging folder where all the files come together to be completed or compiled.
		/// </summary>
		protected string OutputDirectory { get; set; }

		/// <summary>
		/// The final stage output folder specified by the user to publish the final content files to.
		/// </summary>
		protected string PublishDirectory { get; set; }

		/// <summary>
		/// The regular expression to check for illegal file characters before creating files.
		/// </summary>
		protected System.Text.RegularExpressions.Regex IllegalFileCharacters { get; private set; }

		/// <summary>
		/// The export settings.
		/// </summary>
		protected ExportSettings Settings { get; set; }

		/// <summary>
		/// The export configuration details.
		/// </summary>
		protected ExportConfigFile Config { get; set; }

		/// <summary>
		/// Counter indicating the current export step in the export process.
		/// </summary>
		protected int CurrentExportStep { get; set; }
		#endregion

		/// <summary>
		/// Performs the export steps necessary to produce the final exported documentation in the
		/// implementing type.
		/// </summary>
		public abstract void Export();

		/// <summary>
		/// Ensures there is an empty temp directory to proccess the documentation in.
		/// </summary>
		protected void PrepareDirectory(string directory) {
			if (!Directory.Exists(directory)) {
				Directory.CreateDirectory(directory);
			}
			else {
				Directory.Delete(directory, true);
				Directory.CreateDirectory(directory);
			}
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

		protected virtual void Export(Entry current) {
			System.Diagnostics.Debug.WriteLine("SaveXaml: " + current.Name + "[" + current.Key + ", " + current.SubKey + "]");
			System.Diagnostics.Debug.Indent();

			Rendering.XmlRenderer r = Rendering.XmlRenderer.Create(current, this);

			if (r != null) {
				string filename = string.Format("{0}{1}{2}.xml", this.TempDirectory, current.Key, string.IsNullOrEmpty(current.SubKey) ? string.Empty : "-" + this.IllegalFileCharacters.Replace(current.SubKey, string.Empty));
				System.Diagnostics.Debug.WriteLine("filename: " + filename);
				using (System.Xml.XmlWriter writer = XmlWriter.Create(filename)) {
					r.Render(writer);
				}
				System.Diagnostics.Debug.WriteLine("File: " + filename);
			}

			System.Diagnostics.Debug.Unindent();
		}

		/// <summary>
		/// Gets a unique id across this exported live document
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="memberUniqueId">The member unique id.</param>
		/// <param name="typeUniqueId">The type unique id.</param>
		internal void GetUniqueId(CRefPath path, out long memberUniqueId, out long typeUniqueId) {
			System.Diagnostics.Debug.WriteLine("GetUniqueId: " + path.ToString());
			System.Diagnostics.Debug.Indent();

			TypeDef type = null;
			ReflectedMember member = null;
			memberUniqueId = 0;
			typeUniqueId = 0;

			if (path.PathType != CRefTypes.Error) {
				foreach (DocumentedAssembly ass in this.CurrentFiles) {
					type = ass.LoadedAssembly.FindType(path.Namespace, path.TypeName);
					if (type != null)
						break;
				}

				if (type != null) {
					if (path.PathType == CRefTypes.Type) {
						member = type;
					}
					else if (path.PathType == CRefTypes.Property || path.PathType == CRefTypes.Method || path.PathType == CRefTypes.Field || path.PathType == CRefTypes.Event) {
						member = path.FindIn(type);
					}

					if (member != null) {
						memberUniqueId = this.GetUniqueKey(type.Assembly, member);
						typeUniqueId = this.GetUniqueKey(type.Assembly, type);
					}
				}
			}

			System.Diagnostics.Debug.WriteLine("tId: " + typeUniqueId.ToString() + " - mId: " + memberUniqueId.ToString());
			System.Diagnostics.Debug.Unindent();
		}

		/// <summary>
		/// Finds the entry in the document map with the specified key.
		/// </summary>
		/// <param name="key">The key to search for.</param>
		/// <param name="checkChildren">Wether or not to check the child entries</param>
		/// <returns>The entry that relates to the key or null if not found</returns>
		protected Entry FindByKey(long key, string subKey, bool checkChildren) {
			Entry found = null;
			for (int i = 0; i < this.DocumentMap.Count; i++) {
				found = this.DocumentMap[i].FindByKey(key, subKey, checkChildren);
				if (found != null) {
					break;
				}
			}
			return found;
		}

		/// <summary>
		/// Obtains a key that uniquely identifies the member in the library, for all libraries
		/// loaded in to the documenter.
		/// </summary>
		/// <param name="assembly">The assembly</param>
		/// <param name="member">The member</param>
		/// <returns>A long that is unique in the application</returns>
		internal long GetUniqueKey(AssemblyDef assembly, ReflectedMember member) {
			long id = ((long)assembly.UniqueId) << 32;
			id += member.UniqueId;
			return id;
		}

		/// <summary>
		/// Obtains a key that uniquely identifies the assembly in the library, for all libraries
		/// and members loaded in to the documenter.
		/// </summary>
		/// <param name="assembly">The assembly to get the unique identifier for</param>
		/// <returns>A long that is unique in the application</returns>
		internal long GetUniqueKey(AssemblyDef assembly) {
			return ((long)assembly.UniqueId) << 32;
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
		/// Raises the <see cref="E:ExportStep"/> event.
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
