using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	using TheBoxSoftware.Reflection;

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
			this.liveDocument = new LiveDocument();
		}
		#endregion

		#region Methods
		public static void SetLiveDocumentorFile(LiveDocumentorFile current) {
			LiveDocumentorFile.current = current;
		}

		internal LiveDocument Update() {
			this.liveDocument.DocumentedFiles = this.Files;
			this.liveDocument.Update();
			return this.liveDocument;
		}

		/// <summary>
		/// Adds a range of assemblyPaths to the Live Documenter.
		/// </summary>
		/// <param name="assemblyPaths">The file path of the referenced assembly</param>
		/// <param name="fromFile">The file these assemblies were parsed from</param>
		public void Add(List<DocumentedAssembly> assemblies, string fromFile) {
			assemblies.Sort((one, two) => one.Name.CompareTo(two.Name));
			this.Files.AddRange(assemblies);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Obtains the single instance of the LIveDocumentorFile
		/// </summary>
		public static LiveDocumentorFile Singleton {
			get { return LiveDocumentorFile.current; }
		}

		public List<DocumentedAssembly> Files {
			get { return this.files; }
		}

		/// <summary>
		/// The actual document that represents the currently visible
		/// live document.
		/// </summary>
		public LiveDocument LiveDocument { get { return this.liveDocument; } }
		#endregion
	}
}
