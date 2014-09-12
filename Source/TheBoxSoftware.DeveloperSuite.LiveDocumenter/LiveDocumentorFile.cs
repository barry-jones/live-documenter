using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TheBoxSoftware.Documentation;
using TheBoxSoftware.Reflection;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	/// <summary>
	/// Represents a project file, which can be used to save and load the preferences for a LiveDocument.
	/// </summary>
	[Serializable]
	internal sealed class LiveDocumentorFile {
		private static LiveDocumentorFile current;
		private Project project;
		private LiveDocument liveDocument;

		static LiveDocumentorFile() {
			current = new LiveDocumentorFile();
		}

		private LiveDocumentorFile() {
            // a new file is always created with no projects and defaults to C# and the debug configuration
			this.project = new Project();
			this.project.Language = Reflection.Syntax.Languages.CSharp;
			this.project.Configuration = Model.BuildConfigurations.Debug.ToString();
		}

		/// <summary>
		/// Sets the current LiveDocument managed by the LiveDocumentor
		/// </summary>
		/// <param name="current">The LiveDocument to manage.</param>
		public static void SetLiveDocumentorFile(LiveDocumentorFile current) {
			LiveDocumentorFile.current = current;
		}

		/// <summary>
		/// Update the list of DocumentedAssembly files and refreshes the document map.
		/// </summary>
		/// <returns>The updated LiveDocument.</returns>
		internal LiveDocument Update() {
			this.liveDocument = new LiveDocument(this.project.GetAssemblies(), this.project.VisibilityFilters);
			this.liveDocument.Settings.VisibilityFilters = this.project.VisibilityFilters;
			this.liveDocument.Update();
			return this.liveDocument;
		}

		/// <summary>
		/// Opens the specified file in the LiveDocumentorFile.
		/// </summary>
		/// <param name="filename">The filename of the file to open.</param>
		/// <remarks>
		/// When using open, the existing assemblies are cleared. Further opening a
		/// file does not update the document. A call to <see cref="Update"/> is 
		/// required.
		/// </remarks>
		public void Open(string filename) {
			this.project = new Project();
			this.project.Files.Add(filename);

			this.Filename = string.Empty;
			this.HasChanged = true;
		}

		/// <summary>
		/// Adds a new file to the LiveDocumentorFile
		/// </summary>
		/// <param name="filename">The file to add.</param>
		public void Add(string filename) {
			this.Add(new string[] { filename });
		}

		/// <summary>
		/// Adds an array of new files to the LiveDocumentorFile.
		/// </summary>
		/// <param name="files">The files to add.</param>
		public void Add(string[] files) {
			this.project.AddFiles(files);
			this.HasChanged = true;
		}

		/// <summary>
		/// Removes the assembly with the <paramref name="uniqueId"/>.
		/// </summary>
		/// <param name="uniqueId">The assembly to remove.</param>
		public void Remove(long uniqueId) {
			DocumentedAssembly assembly = this.project.GetAssemblies().Find(a => a.UniqueId == uniqueId);

			if (assembly != null) {
				this.project.RemovedAssemblies.Add(assembly.Name);
				this.HasChanged = true;
			}
		}

		/// <summary>
		/// Removes the assembly with the <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The assembly to remove.</param>
		public void Remove(string name) {
			DocumentedAssembly assembly = this.project.GetAssemblies().Find(a => a.Name == name);

			if (assembly != null) {
				this.project.RemovedAssemblies.Add(assembly.Name);
				this.HasChanged = true;
			}
		}

		/// <summary>
		/// Saves the changes to this LDF file to its project file on disk.
		/// </summary>
		public void Save() {
			this.SavaAs(this.Filename);
		}

		/// <summary>
		/// Saves the LDF file as a project file to disk
		/// </summary>
		/// <param name="filename">The filename to save this to.</param>
		public void SavaAs(string filename) {
			// remove the last recent file which will be this
			if (Model.UserApplicationStore.Store.RecentFiles.Count > 0) {
				Model.UserApplicationStore.Store.RecentFiles.RemoveAt(0);
			}

			this.project.Serialize(filename);
			this.HasChanged = false;
			this.Filename = filename;

			Model.UserApplicationStore.Store.RecentFiles.Insert(0,
				new Model.RecentFile(filename, System.IO.Path.GetFileName(filename))
				);
		}

		/// <summary>
		/// Loads an existing Live Documenter Project file and sets it as the current
		/// project.
		/// </summary>
		/// <param name="filename">The filename of the project file.</param>
		/// <returns>The instantiated LiveDocumenterFile</returns>
		public static LiveDocumentorFile Load(string filename) {
			if (System.IO.Path.GetExtension(filename) != ".ldproj")
				throw new ArgumentException("Was only expecting an Live Documenter Project file.");
			if (!System.IO.File.Exists(filename))
				throw new ArgumentException(string.Format("File '{0}' was expected but did not exist.", filename));

			// deserialize it
			Project project = Project.Deserialize(filename);

			// test the files
			string[] missingFiles = project.GetMissingFiles();
			if(missingFiles.Length > 0) {
				string message = string.Format("The following files could not be located: \n\n{0}\n\n Do you wish to continue to load the project? The missing files will be ignored.",
					string.Join("\t\n", missingFiles)
					);
				if(MessageBox.Show(message, "Could not locate files", MessageBoxButton.YesNo) == MessageBoxResult.No) {
					LiveDocumentorFile.SetLiveDocumentorFile(new LiveDocumentorFile());
					return LiveDocumentorFile.Singleton;
				}
				else {
					for(int i = 0; i < missingFiles.Length; i++) {
						project.Files.Remove(missingFiles[i]);
					}
				}
			}

			// convert it and set the LDF as current
			LiveDocumentorFile ldFile = new LiveDocumentorFile();
			ldFile.project = project;
			ldFile.Filename = filename;

			LiveDocumentorFile.SetLiveDocumentorFile(ldFile);
			return ldFile;
		}

		/// <summary>
		/// Clears all of the assemblies from the Live Documenter File.
		/// </summary>
		public void Clear() {
			this.project.Files.Clear();
		}

		/// <summary>
		/// Obtains the single instance of the LIveDocumentorFile
		/// </summary>
		public static LiveDocumentorFile Singleton {
			get { return LiveDocumentorFile.current; }
		}

		/// <summary>
		/// The filename this LDP file was loaded from.
		/// </summary>
		public string Filename { get; set; }

		/// <summary>
		/// Indicates if the files have changed in this project and require saving.
		/// </summary>
		public bool HasChanged { get; set; }

		/// <summary>
		/// Indicates if this LiveDocumenterFile has assemblies that can be documented.
		/// </summary>
		public bool HasFiles {
			get {
				return this.project.GetAssemblies().Count + this.project.RemovedAssemblies.Count > 0; 
			}
		}

		/// <summary>
		/// Gets a reference to the underlying project.
		/// </summary>
		internal Project UnerlyingProject {
			get { return this.project; }
		}

		/// <summary>
		/// The actual document that represents the currently visible
		/// live document.
		/// </summary>
		public LiveDocument LiveDocument { get { return this.liveDocument; } }

		/// <summary>
		/// The visibility filters
		/// </summary>
		public List<Reflection.Visibility> Filters {
			get { return this.project.VisibilityFilters; }
			set {
				Reflection.Visibility[] current = this.project.VisibilityFilters.ToArray();
				Reflection.Visibility[] newFilters = value.ToArray();
				if (!current.SequenceEqual(newFilters)) {
					this.HasChanged = true;
					this.project.VisibilityFilters = value;
				}
			}
		}

		/// <summary>
		/// Specified the syntax language
		/// </summary>
		public Reflection.Syntax.Languages Language {
			get { return this.project.Language; }
			set {
				if (this.project.Language != value) {
					this.project.Language = value;
					this.HasChanged = true;
				}
			}
		}

		/// <summary>
		/// Specified the build configuration
		/// </summary>
		public Model.BuildConfigurations Configuration {
			get {
				if (string.IsNullOrEmpty(this.project.Configuration)) {
					return Model.BuildConfigurations.Debug;
				}
				else {
					return (Model.BuildConfigurations)Enum.Parse(typeof(Model.BuildConfigurations), this.project.Configuration);
				}
			}
			set {
				if (this.project.Configuration != value.ToString()) {
					this.HasChanged = true;
					this.project.Configuration = value.ToString();
				}
			}
		}

		/// <summary>
		/// The users last output configuration for this project.
		/// </summary>
		public string OutputLocation {
			get { return this.project.OutputLocation; }
			set {
				this.HasChanged = this.HasChanged || this.project.OutputLocation != value;
				this.project.OutputLocation = value;
			}
		}
	}
}
