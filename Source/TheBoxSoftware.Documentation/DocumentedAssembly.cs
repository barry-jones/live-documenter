using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation {
	using TheBoxSoftware.Reflection;

	/// <summary>
	/// An assembly that is being documented by the application. Acts as a wrapper to the file
	/// and associated xml file.
	/// </summary>
	/// <seealso cref="AssemblyDef" />
	/// <seealso cref="Comments.XmlCodeCommentFile" />
	public sealed class DocumentedAssembly {
        // 32 bytes
		private AssemblyDef assembly;
        private string filename;
        private DateTime timeLoaded;
        private string name;
        private string xmlFilename;
        private List<string> referencedAssemblies;
        private long uniqueId;

		#region Constructors
		/// <summary>
		/// Default constructor initialises an empty instance of the DocumentedAssembly
		/// class.
		/// </summary>
		public DocumentedAssembly() { }

		/// <summary>
		/// Initialises a new instance of the DocumentedAssembly class where the FileName
		/// of the assembly is known.
		/// </summary>
		/// <param name="fileName">The full filepath and name of the assembly.</param>
		/// <exception cref="ArgumentNullException">The FileName is null or empty.</exception>
		/// <remarks>
		/// When only the fileName is provided the xml comment file is assumed to be the same
		/// file (i.e. path and name) as the assembly with the .xml extension instead of dll
		/// for example.
		/// </remarks>
		public DocumentedAssembly(string fileName) {
			if (string.IsNullOrEmpty(fileName))
				throw new ArgumentNullException("fileName");
			this.FileName = fileName;
			this.XmlFileName = System.IO.Path.ChangeExtension(fileName, "xml");
			this.Name = System.IO.Path.GetFileNameWithoutExtension(fileName);
		}

		/// <summary>
		/// Initialises a new instance of the DocumentedAssembly class where the FileName
		/// of the assembly is known.
		/// </summary>
		/// <param name="fileName">The full filepath and name of the assembly.</param>
		/// <param name="documentationFile">The full filepath and name of the associated xml documentation file.</param>
		/// <exception cref="ArgumentNullException">The FileName is null or empty.</exception>
		/// <remarks>
		/// When only the fileName is provided the xml comment file is assumed to be the same
		/// file (i.e. path and name) as the assembly with the .xml extension instead of dll
		/// for example.
		/// </remarks>
		public DocumentedAssembly(string fileName, string documentationFile) {
			if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");

			// no doc file, assume output in default location
			if (string.IsNullOrEmpty(documentationFile)) {
				documentationFile = System.IO.Path.ChangeExtension(fileName, "xml");
			}

			this.FileName = fileName;
			this.XmlFileName = documentationFile;
			this.Name = System.IO.Path.GetFileNameWithoutExtension(fileName);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Checks if this documented assembly has been modified since we last loaded
		/// it.
		/// </summary>
		/// <returns>True if it has changed else false.</returns>
		public bool HasAssemblyBeenModified() {
			DateTime lastWriteTime = System.IO.File.GetLastWriteTime(this.FileName);
			return this.IsCompiled && this.TimeLoaded < lastWriteTime;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the full name and path of the assembly being documented.
		/// </summary>
		public string FileName {
            get { return this.filename; }
            set { this.filename = value; }
        }

		/// <summary>
		/// Gets or sets the date and time the file was last loaded.
		/// </summary>
		public DateTime TimeLoaded {
            get { return this.timeLoaded; }
            private set { this.timeLoaded = value; }
        }

		/// <summary>
		/// The name of the assembly as it would appear in assembly reference metadata entries.
		/// </summary>
		public string Name {
            get { return this.name; }
            set { this.name = value; }
        }

		/// <summary>
		/// Gets the full path and filename for the associated XML code comments file.
		/// </summary>
		public string XmlFileName {
            get { return this.xmlFilename; }
            set { this.xmlFilename = value; }
        }

		/// <summary>
		/// A collection of assemblies that are known to be referenced from this assembly
		/// this can only be populated when this documented assembly has been loaded from
		/// a project file directly or via a solution. When it is not the assembly will have
		/// to be queried for its references.
		/// </summary>
		public List<string> ReferencedAssemblies {
            get { return this.referencedAssemblies; }
            set { this.referencedAssemblies = value; }
        }

		/// <summary>
		/// Indicates that the file exists on disk and is compiled.
		/// </summary>
		public bool IsCompiled { get { return System.IO.File.Exists(this.FileName); } }

		/// <summary>
		/// The unique id for this Assembly
		/// </summary>
		public long UniqueId {
            get { return this.uniqueId; }
            set { this.uniqueId = value; }
        }

		/// <summary>
		/// A reference to the assembly after it has been loaded.
		/// </summary>
		public AssemblyDef LoadedAssembly {
			get {
				return this.assembly;
			}
			set {
				this.assembly = value;
				this.TimeLoaded = DateTime.Now;
			}
		}
		#endregion
	}
}
