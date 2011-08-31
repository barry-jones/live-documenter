using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Xml.Serialization;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Documentation;

	/// <summary>
	/// Represents a project file, which can be used to save and load
	/// the preferences for a LiveDocument.
	/// </summary>
	[Serializable]
	public sealed class LiveDocumentorFile {
		private static LiveDocumentorFile current;
		private LiveDocument liveDocument;
		private List<DocumentedAssembly> files;

		#region Constructors
		/// <summary>
		/// Static constructor
		/// </summary>
		static LiveDocumentorFile() {
			current = new LiveDocumentorFile();
		}

		/// <summary>
		/// Private constructor for initialising the single instance.
		/// </summary>
		private LiveDocumentorFile() {
			this.files = new List<DocumentedAssembly>();
			// this.liveDocument = new LiveDocument();
		}
		#endregion

		#region Methods
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
			if (this.liveDocument == null) {
				this.liveDocument = new LiveDocument(this.files);
			}
			this.liveDocument.Assemblies = this.files;
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
			List<DocumentedAssembly> readFiles = new List<DocumentedAssembly>();
			readFiles.AddRange(
				InputFileReader.Read(
					filename, 
					Model.UserApplicationStore.Store.Preferences.BuildConfiguration.ToString()
					)
				);

			readFiles.Sort((one, two) => one.Name.CompareTo(two.Name));
			this.files.Clear();
			this.files.AddRange(readFiles);
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
			List<DocumentedAssembly> readFiles = new List<DocumentedAssembly>();
			foreach (string filename in files) {
				readFiles.AddRange(
					InputFileReader.Read(
						filename, 
						Model.UserApplicationStore.Store.Preferences.BuildConfiguration.ToString()
						)
					);
			}

			for(int i = readFiles.Count - 1; i >= 0; i--) {
				DocumentedAssembly assembly = this.files.Find(d => d.FileName == readFiles[i].FileName);
				if(assembly != null) {
					readFiles.RemoveAt(i);
				}
			}

			this.files.AddRange(readFiles);
			this.files.Sort((one, two) => one.Name.CompareTo(two.Name));
			this.HasChanged = true;
		}

		/// <summary>
		/// Removes the specified <paramref name="assembly"/> from the LiveDocumentorFile.
		/// </summary>
		/// <param name="assembly">The assembly to remove.</param>
		public void Remove(DocumentedAssembly assembly) {
			if(this.files.Contains(assembly)) {
				this.files.Remove(assembly);
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
			// convert the LDF to a LDP
			LiveDocumenterProject project = new LiveDocumenterProject();
			foreach(DocumentedAssembly assembly in this.files) {
				project.Files.Add(assembly.FileName);
			}

			project.Serialize(filename);
			this.HasChanged = false;
			this.Filename = filename;
		}
		
		/// <summary>
		/// Loads an existing Live Documenter Project file and sets it as the current
		/// project.
		/// </summary>
		/// <param name="filename">The filename of the project file.</param>
		/// <returns>The instantiated LiveDocumenterFile</returns>
		public static LiveDocumentorFile Load(string filename) {
			if(System.IO.Path.GetExtension(filename) != ".ldp")
				throw new ArgumentException("Was only expecting an Live Documenter Project file.");
			if(!System.IO.File.Exists(filename))
				throw new ArgumentException(string.Format("File '{0}' was expected but did not exist.", filename));
			
			// deserialize it
			LiveDocumenterProject project = LiveDocumenterProject.Deserialize(filename);

			// convert it and set the LDF as current
			LiveDocumentorFile ldFile = new LiveDocumentorFile();
			foreach(string file in project.Files) {
				ldFile.files.Add(new DocumentedAssembly(file));
			}
			ldFile.Filename = filename;
			LiveDocumentorFile.SetLiveDocumentorFile(ldFile);
			return ldFile;
		}

		/// <summary>
		/// Clears all of the assemblies from the Live Documenter File.
		/// </summary>
		public void Clear() {
			this.files.Clear();
		}
		#endregion

		#region Properties
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
		/// The actual document that represents the currently visible
		/// live document.
		/// </summary>
		public LiveDocument LiveDocument { get { return this.liveDocument; } }
		#endregion

		#region Inernals
		[Serializable]
		[XmlRoot("project")]
		public sealed class LiveDocumenterProject {
			public LiveDocumenterProject() {
				this.Files = new List<string>();
			}

			[XmlArray("files")]
			[XmlArrayItem("file")]
			public List<string> Files { get; set; }

			public void Serialize(string toFile) {
				using(FileStream fs = new FileStream(toFile, FileMode.OpenOrCreate)) {
					fs.SetLength(0); // clean up all contents
					XmlSerializer serializer = new XmlSerializer(typeof(LiveDocumenterProject));
					serializer.Serialize(fs, this);
				}
			}

			public static LiveDocumenterProject Deserialize(string fromFile) {
				using(FileStream fs = new FileStream(fromFile, FileMode.Open)) {
					XmlSerializer serializer = new XmlSerializer(typeof(LiveDocumenterProject));
					return (LiveDocumenterProject)serializer.Deserialize(fs);
				}
			}
		}
		#endregion
	}
}
