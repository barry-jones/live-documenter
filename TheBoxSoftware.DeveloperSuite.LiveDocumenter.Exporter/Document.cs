using System;
using System.Collections.Generic;
using System.Text;
using External = TheBoxSoftware.Documentation;
using System.IO;

namespace TheBoxSoftware.Exporter {
	/// <summary>
	/// Represents the Documentation for a group of assemblies.
	/// </summary>
	/// <include file='Documentation\document.xml' path='member[@name="Document"]/*'/>
	public sealed class Document {
		private External.Document baseDocument;

		#region Constructors
		/// <summary>
		/// Initialises a new Document
		/// </summary>
		public Document() { }

		/// <summary>
		/// Initialises a new Document
		/// </summary>
		/// <param name="file">The</param>
		public Document(string file) {
			this.Filename = file;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Refreshes the current document to read in changes to the underlying files
		/// or to build the Document when loading for the first time.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// The <see cref="Filename"/> was not set or did not point to a correct file.
		/// </exception>
		public void Refresh() {
			if(string.IsNullOrEmpty(this.Filename))
				throw new InvalidOperationException("The Filename was null or empty. The filename is required.");
			if(!System.IO.File.Exists(this.Filename))
				throw new InvalidOperationException(string.Format("The Filename {0} did not exist on disk.", this.Filename));

			List<External.DocumentedAssembly> files = new List<External.DocumentedAssembly>();
			External.DocumentSettings settings = new External.DocumentSettings();

			if(System.IO.Path.GetExtension(this.Filename) == ".ldproj") {
				External.Project p = External.Project.Deserialize(this.Filename);
				foreach(string file in p.Files) {
					files.Add(new External.DocumentedAssembly(file));
				}
				foreach(Reflection.Visibility filter in p.VisibilityFilters){
					settings.VisibilityFilters.Add(filter);
				}
			}
			else {
				files = External.InputFileReader.Read(this.Filename, "Release");
			}

			this.baseDocument = new External.Document(files);
			this.baseDocument.Settings = settings;

			this.baseDocument.UpdateDocumentMap();
		}

		/// <summary>
		/// Exports the entire documentation set to the <paramref name="toDestination"/> 
		/// directory.
		/// </summary>
		/// <param name="toDestination">The directory to write the files to.</param>
		public void Export(string toDestination) {
		}

		public Stream GetDocumentationFor(string member) {
			return null;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the name of the file being documented.
		/// </summary>
		public string Filename { get; set; }
		#endregion
	}
}
