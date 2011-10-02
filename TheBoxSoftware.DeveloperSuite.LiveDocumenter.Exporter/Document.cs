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
		/// <param name="document">The document to manage and export</param>
		public Document(string document, string format) {
			this.Filename = document;
			this.Format = format;
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
			Documentation.Exporting.ExportConfigFile ldec = TheBoxSoftware.Documentation.Exporting.ExportConfigFile.Create(this.Format);
			Documentation.Exporting.ExportSettings exportSettings = new External.Exporting.ExportSettings();
			exportSettings.Settings = this.baseDocument.Settings;

			Documentation.Exporting.Exporter exporter = Documentation.Exporting.Exporter.Create(this.baseDocument, exportSettings, ldec);
			exporter.Export();
		}

		/// <summary>
		/// Gets the documentation for the specified <paramref name="member"/>.
		/// </summary>
		/// <param name="member">The member to output, this can be in cref or full member format.</param>
		/// <include file='Documentation\document.xml' path='member[@name="Document.GetDocumentationFor"]/*'/>
		public Stream GetDocumentationFor(string cref) {
			if (string.IsNullOrEmpty(this.Format)) throw new InvalidOperationException("The Format was not specified and is requried to export the documentation.");
			if (!File.Exists(this.Format)) throw new InvalidOperationException("The Format specified could not be located.");

			// convert string to cref
			Reflection.Comments.CRefPath path = Reflection.Comments.CRefPath.Parse(cref);
			Documentation.Entry member = this.baseDocument.Find(path);
			
			Documentation.Exporting.ExportConfigFile ldec = TheBoxSoftware.Documentation.Exporting.ExportConfigFile.Create(this.Format);
			Documentation.Exporting.ExportSettings exportSettings = new External.Exporting.ExportSettings();
			exportSettings.Settings = this.baseDocument.Settings;

			Documentation.Exporting.Exporter exporter = Documentation.Exporting.Exporter.Create(this.baseDocument, exportSettings, ldec);
			return exporter.ExportMember(member);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the name of the file being documented.
		/// </summary>
		public string Filename { get; set; }

		/// <summary>
		/// Gets or sets the LDEC file used to format/render the exported content.
		/// </summary>
		public string Format { get; set; }
		#endregion
	}
}
